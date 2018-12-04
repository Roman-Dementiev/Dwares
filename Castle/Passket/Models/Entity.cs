﻿using System;
using Dwares.Dwarf;


namespace Passket.Models
{
	public interface IEntity
	{
		uint Id { get; set; }
		string Icon { get; }
		string Name { get; }
		string Info { get; }
	}

	public class Entity : IEntity
	{
		//static ClassRef @class = new ClassRef(typeof(Entity));

		public Entity()
		{
			//Debug.EnableTracing(@class);
		}

		public uint Id { get; set; }
		public string Icon { get; set; }
		public string Name { get; set; }
		public string Info { get; set; }
	}
}
