using System;
using System.Collections.Generic;
using KSP;

namespace AdvancedActiveRadiators
{
	public class ModuleAdvancedActiveRadiator : ModuleActiveRadiator
	{
		public List<AttachNode> attachNodes = new List<AttachNode>();

		public List<string> attachNodeNames = new List<string>();

		public List<ResourceRate> resources;

		public ModuleAdvancedActiveRadiator ()
		{
		}

		public void OnStart()
		{
		}

		public new void FixedUpdate()
		{
			base.FixedUpdate();
			part.skinTemperature += (part.thermalInternalFlux * part.skinThermalMassRecip * TimeWarp.fixedDeltaTime);
			part.thermalInternalFlux = 0.0;
		}

		public override string GetInfo ()
		{
			string s;
			s = "Requirements:\n";
			foreach (ResourceRate resource in resources)
			{
				double _rate = resource.rate;
				if(_rate > 1)
					s += "  " + resource.name + ": " + _rate.ToString ("F2") + " " + "/s\n";
				else if(_rate > 0.01666667f)
					s += "  " + resource.name + ": " + (_rate * 60).ToString ("F2") + " " +  "/m\n";
				else
					s += "  " + resource.name + ": " + (_rate * 3600).ToString ("F2") + " " + "/h\n";
			}
			
			return s;
		}

		public override void OnLoad (ConfigNode node)
		{
			base.OnLoad (node);
			foreach (ConfigNode n in node.GetNodes ("RESOURCE")) 
			{
				if(n.HasValue ("name") && n.HasValue ("rate")) 
				{
					double rate;
					string unitName = "";
					if (n.HasValue ("unitName"))
						unitName = n.GetValue ("unitName");
					else
						unitName = n.GetValue("name");
					double.TryParse (n.GetValue ("rate"), out rate);
					
					resources.Add (new ResourceRate(n.GetValue("name"), rate, unitName));
					print ("adding RESOURCE " + n.GetValue("name") + " = " + rate.ToString());
				}
			}
		}

		public class ResourceRate
		{
			public string name;
			public string unitName;
			public double rate;
			public int id
			{
				get 
				{
					return name.GetHashCode ();
				}
			}
			public ResourceRate(string name, double rate, string unitName)
			{
				this.name = name;
				this.rate = rate;
				this.unitName = unitName;
			}
		}
	}
}

