using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dwares.Dwarf;
using Dwares.Dwarf.Collections;
using DraggableListView.Models;


namespace DraggableListView.Services
{
	public class MockDataStore : IDataStore
	{
		//static ClassRef @class = new ClassRef(typeof(MockDataStore));

		public MockDataStore()
		{
			//Debug.EnableTracing(@class);

			int ordinal = 0;
			Squad.SquadLeader.Ordinal = ++ordinal;

			foreach (var team in Squad.Teams) {
				team.TeamLeader.Ordinal = ++ordinal;

				int teamOrdinal = 0;
				foreach (var marine in team) {
					marine.Ordinal = ++ordinal;
					marine.TeamOrdinal = ++teamOrdinal;
				}
			}
		}

		public Task LoadSquad(Squad squad)
		{
			squad.Name = Squad.Name;
			squad.SquadLeader = Squad.SquadLeader;

			squad.Teams.Clear();
			foreach (var team in Squad.Teams) {
				squad.Teams.Add(team);
			}

			return Task.CompletedTask;
		}


		public async Task LoadMarines(ICollection<Marine> collection)
		{
			collection.Clear();

			await AddMarine(collection, Squad.SquadLeader);

			foreach (var team in Squad.Teams) {
				await AddMarine(collection, team.TeamLeader);

				foreach (var marine in team) {
					await AddMarine(collection, marine);
				}
			}
		}

		async Task AddMarine(ICollection<Marine> collection, Marine marine)
		{
			collection.Add(marine);
			await Task.Delay(100);
		}

		//List<Marine> Marines = new List<Marine> {
		//		new Marine { Ordinal = 1, FirstName = "Theo", LastName = "Robrecht", Rank = Ranks.Sergeant },
		//		new Marine { Ordinal = 2, FirstName = "Ardito", LastName = "Prabhakar", Rank = Ranks.Corporal },
		//		new Marine { Ordinal = 3, FirstName = "Gilbert", LastName = "Lupus", Rank = Ranks.Corporal },
		//		new Marine { Ordinal = 4, FirstName = "Epaphras", LastName = "Teige", Rank = Ranks.Corporal },
		//		new Marine { Ordinal = 5, FirstName = "Gerolamo", LastName = "Brett", Rank = Ranks.LanceCorporal },
		//		new Marine { Ordinal = 6, FirstName = "Neo", LastName = "Devaraja", Rank = Ranks.LanceCorporal },
		//		new Marine { Ordinal = 7, FirstName = "Roberto", LastName = "Ankit", Rank = Ranks.LanceCorporal },
		//		new Marine { Ordinal = 8, FirstName = "Krishna", LastName = "Aryan", Rank = Ranks.LanceCorporal },
		//		new Marine { Ordinal = 9, FirstName = "Kepheus", LastName = "Arnold", Rank = Ranks.LanceCorporal },
		//		new Marine { Ordinal = 10, FirstName = "Manlius", LastName = "Maikel", Rank = Ranks.LanceCorporal },
		//		new Marine { Ordinal = 11, FirstName = "Mpho", LastName = "Evander", Rank = Ranks.PrivateFirstClass },
		//		new Marine { Ordinal = 12, FirstName = "Carran", LastName = "Otho", Rank = Ranks.PrivateFirstClass },
		//		new Marine { Ordinal = 13, FirstName = "Manas", LastName = "Oddr", Rank = Ranks.Private },
		//		new Marine { Ordinal = 14, FirstName = "Herb", LastName = "Dionysos", Rank = Ranks.Private },
		//};

		Squad Squad = new Squad {
			Name = "First Squad",
			SquadLeader = new Marine { FirstName = "Theo", LastName = "Robrecht", Rank = Ranks.Sergeant },
			Teams = new GroupedOrderableCollection<FireTeam> {
				new FireTeam("Alpha", new Marine { FirstName = "Ardito", LastName = "Prabhakar", Rank = Ranks.Corporal}) {
					new Marine { FirstName = "Gerolamo", LastName = "Brett", Rank = Ranks.LanceCorporal },
					new Marine { FirstName = "Neo", LastName = "Devaraja", Rank = Ranks.LanceCorporal },
					new Marine { FirstName = "Mpho", LastName = "Evander", Rank = Ranks.PrivateFirstClass },
					new Marine { FirstName = "Herb", LastName = "Dionysos", Rank = Ranks.Private },
					new Marine { FirstName = "Gerolamo", LastName = "Brett", Rank = Ranks.LanceCorporal },
					new Marine { FirstName = "Neo", LastName = "Devaraja", Rank = Ranks.LanceCorporal },
					new Marine { FirstName = "Roberto", LastName = "Ankit", Rank = Ranks.LanceCorporal },
					new Marine { FirstName = "Krishna", LastName = "Aryan", Rank = Ranks.LanceCorporal },
					new Marine { FirstName = "Kepheus", LastName = "Arnold", Rank = Ranks.LanceCorporal },
					new Marine { FirstName = "Manlius", LastName = "Maikel", Rank = Ranks.LanceCorporal },
					new Marine { FirstName = "Mpho", LastName = "Evander", Rank = Ranks.PrivateFirstClass },
					new Marine { FirstName = "Carran", LastName = "Otho", Rank = Ranks.PrivateFirstClass },
					new Marine { FirstName = "Manas", LastName = "Oddr", Rank = Ranks.Private },
					new Marine { FirstName = "Herb", LastName = "Dionysos", Rank = Ranks.Private },
				},
				new FireTeam("Bravo", new Marine { FirstName = "Gilbert", LastName = "Lupus", Rank = Ranks.Corporal }) {
					new Marine { FirstName = "Roberto", LastName = "Ankit", Rank = Ranks.LanceCorporal },
					new Marine { FirstName = "Krishna", LastName = "Aryan", Rank = Ranks.LanceCorporal },
					new Marine { FirstName = "Carran", LastName = "Otho", Rank = Ranks.PrivateFirstClass },
				},
				new FireTeam("Charlie", new Marine { FirstName = "Ardito", LastName = "Prabhakar", Rank = Ranks.Corporal }) {
					new Marine { FirstName = "Kepheus", LastName = "Arnold", Rank = Ranks.LanceCorporal },
					new Marine { FirstName = "Manlius", LastName = "Maikel", Rank = Ranks.LanceCorporal },
					new Marine { FirstName = "Manas", LastName = "Oddr", Rank = Ranks.Private },
				}
			}
		};
	}
}
