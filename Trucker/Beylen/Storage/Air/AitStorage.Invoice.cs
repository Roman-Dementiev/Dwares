using System;
using System.Threading.Tasks;
using Dwares.Dwarf;
using Dwares.Drudge.Airtable;
using Beylen.Models;


namespace Beylen.Storage.Air
{
	public partial class AirStorage : AirClient, IAppStorage
	{
		public async Task LoadInvoices(string carId)
		{
			var query = new QyeryBuilder();
			query.SetSortByField(InvoiceRecord.SEQ);
			if (string.IsNullOrEmpty(carId)) {
				query.FilterByFormula = "{Date} >= TODAY()";
			} else {
				query.FilterByFormula = $"And({{Date}} = TODAY(), {{Car}}=\"{carId}\")";
			}

			//var records = await InvoicesTable.ListRecords(sortField: InvoiceRecord.SEQ);
			var result = await InvoicesTable.List(query);
			var records = result?.Records;
			if (records == null) {
				Debug.Print("InvoicesTable.List() returned no records");
				return;
			}
			var invoices = AppScope.Instance.Invoices;
			int ord = 0;
			foreach (var rec in records) {
				var customer = AppScope.GetCustomer(rec.Customer);
				if (customer == null) {
					Debug.Print($"## AirStorage.LoadInvoices(): Unknown Customer {Dw.ToString(rec.Customer)}");
					continue;
				}

				var invoice = new Invoice {
					RecordId = rec.Id,
					Seq = rec.Seq,
					Ordinal = ++ord,
					Date = rec.Date,
					CarId = rec.Car,
					Number = rec.Number,
					Customer = customer,
					Notes = rec.Notes
				};

				var list = await ArticlesTable.FilterRecords($"{{{ArticleRecord.INVOICE_SEQ}}} = {invoice.Seq}");
				foreach (var art in list.Records) {
					var produce = AppScope.GetProduce(art.Produce);
					if (produce == null) {
						Debug.Print($"## AirStorage.LoadInvoices(): Unknown Produce {Dw.ToString(art.Produce)}");
						continue;
					}

					var article = new Article {
						RecordId = art.Id,
						Produce = produce,
						Quantity = art.Quantity,
						Unit = art.Unit,
						UnitPrice = art.UnitPrice,
						TotalPrice = art.TotalPrice,
						Note = art.Note
					};

					invoice.Articles.Add(article);
				}

				invoices.Add(invoice);
			}
		}

		public async Task<string> VerifyNumber(string number)
		{
			var head = number.Substring(0, 4);
			int numb = 1; //int.Parse(number.Substring(4));

			var records = await InvoicesTable.ListRecords();
			foreach (var rec in records) {
				if (rec.Number.StartsWith(head)) {
					var n = int.Parse(rec.Number.Substring(4));
					if (n >= numb)
						numb = n+1;
				}
			}

			return head + string.Format("{0,2:D2}", numb);
		}

		public async Task NewInvoice(Invoice invoice)
		{
			var rec = new InvoiceRecord {
				Date = invoice.Date,
				Number = await VerifyNumber(invoice.Number),
				Car = invoice.CarId,
				Customer = invoice.Customer.CodeName,
				Notes = invoice.Notes
			};

			rec = await InvoicesTable.CreateRecord(rec);

			invoice.RecordId = rec.Id;
			invoice.Seq = rec.Seq;
			invoice.Number = rec.Number;
			invoice.Ordinal = AppScope.Instance.Invoices.Count + 1;

			foreach (var article in invoice.Articles) {
				var art = new ArticleRecord {
					InvoiceSeq = invoice.Seq,
					Produce = article.Produce.Name,
					Quantity = article.Quantity,
					Unit = article.Unit,
					UnitPrice = article.UnitPrice,
					TotalPrice = article.TotalPrice
				};

				art = await ArticlesTable.CreateRecord(art);
				article.RecordId = art.Id;
			}

			AppScope.Instance.Invoices.Add(invoice);
		}

		public Task UpdateInvoice(Invoice invoice)
		{
			return Task.CompletedTask;
		}
	}
}
