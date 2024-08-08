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
    public partial class entitystorematerialised_CoreTask : Interneuron.CareRecord.Infrastructure.Domain.EntityBase
    {
        public string TaskId { get; set; }
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
        public string Correlationid { get; set; }
        public string Correlationtype { get; set; }
        public string PersonId { get; set; }
        public string Tasktype { get; set; }
        public string Taskdetails { get; set; }
        public string Taskcreatedby { get; set; }
        public DateTime? Taskcreateddatetime { get; set; }
        public string Taskname { get; set; }
        public string Allocatedto { get; set; }
        public string Notes { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
        public string Owner { get; set; }
        public string EncounterId { get; set; }
        public DateTime? Duedate { get; set; }
        public DateTime? Allocateddatetime { get; set; }
        public DateTime? Ownerassigneddatetime { get; set; }
        public string ClinicalsummaryId { get; set; }
    }
}
