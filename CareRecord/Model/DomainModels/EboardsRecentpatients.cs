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
    public partial class baseview_EboardsRecentpatients : Interneuron.CareRecord.Infrastructure.Domain.EntityBase
    {
        public string Wardcode { get; set; }
        public string Warddiplay { get; set; }
        public string Currentwardcode { get; set; }
        public string Currentwarddisplay { get; set; }
        public string PersonId { get; set; }
        public string Nhsnumber { get; set; }
        public string Mrn { get; set; }
        public string Titletext { get; set; }
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string Familyname { get; set; }
        public string Fullname { get; set; }
        public string Preferredname { get; set; }
        public string Dateofbirth { get; set; }
        public double? Ageinyears { get; set; }
        public TimeSpan? Agetext { get; set; }
        public string Dateofdeath { get; set; }
        public string Gender { get; set; }
        public bool? Deathindicator { get; set; }
        public string EncounterId { get; set; }
        public string Patienttypecode { get; set; }
        public string Patientclasstext { get; set; }
        public string Consultingdoctorid { get; set; }
        public string Consultingdoctortext { get; set; }
        public DateTime? Admitdatetime { get; set; }
        public string Admitdateformatted { get; set; }
        public DateTime? Dischargedatetime { get; set; }
        public string Episodestatuscode { get; set; }
        public string Patientbanner { get; set; }
        public string ColPatient { get; set; }
        public string Bedsortstring { get; set; }
    }
}
