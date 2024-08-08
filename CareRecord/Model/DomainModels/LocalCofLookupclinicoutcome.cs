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

namespace Interneuron.CareRecord.Model.DomainModels
{
    public partial class entitystorematerialised_LocalCofLookupclinicoutcome : Interneuron.CareRecord.Infrastructure.Domain.EntityBase
    {
        public string CofLookupclinicoutcomeId { get; set; }
        public string RowId { get; set; }
        public int? Sequenceid { get; set; }
        public string Contextkey { get; set; }
        public DateTime? Createdtimestamp { get; set; }
        public DateTime? Createddate { get; set; }
        public string Createdsource { get; set; }
        public string Createdmessageid { get; set; }
        public string Createdby { get; set; }
        public short? Recordstatus { get; set; }
        public string Timezonename { get; set; }
        public int? Timezoneoffset { get; set; }
        public string Tenant { get; set; }
        public bool? Ispattreat { get; set; }
        public string Description { get; set; }
        public string Nationalrttcode { get; set; }
        public bool? Isclinoutdischarge { get; set; }
        public bool? Isrefint { get; set; }
        public string Localrttcode { get; set; }
        public bool? Isfirstdeft { get; set; }
        public bool? Isrefext { get; set; }
        public bool? Notrereq { get; set; }
        public int? Orderby { get; set; }
        public bool? Isadmittod { get; set; }
        public bool? Isactivertt { get; set; }
        public bool? Isfollup { get; set; }
        public bool? Isdectre { get; set; }
        public bool? Isdecisiontotreat { get; set; }
        public bool? Ispatmon { get; set; }
        public bool? Isnotactivertt { get; set; }
        public string Idpk { get; set; }
    }
}
