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
    public partial class entitystorematerialised_CoreProvider : Interneuron.CareRecord.Infrastructure.Domain.EntityBase
    {
        public string ProviderId { get; set; }
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
        public string Firstname { get; set; }
        public string Grade { get; set; }
        public string Lastname { get; set; }
        public string Middlename { get; set; }
        public string Title { get; set; }
        public string Prefix { get; set; }
        public string Emailid { get; set; }
        public string Phonenumber { get; set; }
        public string Suffix { get; set; }
        public string Employer { get; set; }
        public string Role { get; set; }
        public string Notes { get; set; }
        public string Userid { get; set; }
        public string Providertypecode { get; set; }
        public string Providertypetext { get; set; }
        public string Statuscode { get; set; }
        public string Statustext { get; set; }
        public string Jobtitle { get; set; }
        public DateTime? Startdate { get; set; }
        public DateTime? Enddate { get; set; }
        public string Organisationid { get; set; }
        public string Fullname { get; set; }
    }
}
