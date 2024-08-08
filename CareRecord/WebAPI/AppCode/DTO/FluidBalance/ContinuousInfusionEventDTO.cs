 //Interneuron synapse

//Copyright(C) 2024 Interneuron Limited

//This program is free software: you can redistribute it and/or modify
//it under the terms of the GNU General Public License as published by
//the Free Software Foundation, either version 3 of the License, or
//(at your option) any later version.

//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.

//See the
//GNU General Public License for more details.

//You should have received a copy of the GNU General Public License
//along with this program.If not, see<http://www.gnu.org/licenses/>.
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Interneuron.CareRecord.API.AppCode.DTO.FluidBalance
{
    public class ContinuousInfusionEventDTO
    {
		public string continuousinfusionevent_id { get; set; }
		public string continuousinfusion_id { get; set; }
		public string eventtype { get; set; }
		public string datetime { get; set; }
		public string eventcorrelationid { get; set; }
		public string addedby { get; set; }
		public string modifiedby { get; set; }
		public string deletecorrelationid { get; set; }
	}
}
