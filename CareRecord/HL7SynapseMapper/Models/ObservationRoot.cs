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
﻿using Hl7.Fhir.Model;
using Interneuron.CareRecord.Infrastructure.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Interneuron.CareRecord.HL7SynapseHandler.Service.Models
{
    public class ObservationRoot : SynapseResource
    {
        public ObservationRoot()
        {

        }
        public ObservationEventDTO ObservationEvent { get; set; }

        public List<ObservationDTO> Observations { get; set; }
    }

    public class ObservationDTO
    {
        public ObservationDTO()
        {

        }
        public string Observation_Id { get; set; }
        public string RowId { get; set; }
        public int Sequenceid { get; set; }
        public string Contextkey { get; set; }
        public DateTime? Createdtimestamp { get; set; }
        public DateTime? Createddate { get; set; }
        public string Createdsource { get; set; }
        public string Createdmessageid { get; set; }
        public string _Createdby { get; set; }
        public short? Recordstatus { get; set; }
        public string Timezonename { get; set; }
        public int? Timezoneoffset { get; set; }
        public string Tenant { get; set; }
        public string Units { get; set; }
        public string Symbol { get; set; }
        public DateTime? Timerecorded { get; set; }
        public string Observationevent_Id { get; set; }
        public string Observationtype_Id { get; set; }
        public string Observationtypemeasurement_Id { get; set; }
        public object Value { get; set; }
        public bool? Hasbeenammended { get; set; }
        public string Eventcorrelationid { get; set; }
    }

    public class ObservationEventDTO
    {
        public ObservationEventDTO()
        {

        }
        public string Observationevent_Id { get; set; }
        public string RowId { get; set; }
        public int Sequenceid { get; set; }
        public string Contextkey { get; set; }
        public DateTime? Createdtimestamp { get; set; }
        public DateTime? Createddate { get; set; }
        public string Createdsource { get; set; }
        public string Createdmessageid { get; set; }
        public string _Createdby { get; set; }
        public short? Recordstatus { get; set; }
        public string Timezonename { get; set; }
        public int? Timezoneoffset { get; set; }
        public string Tenant { get; set; }
        public string Person_Id { get; set; }
        public DateTime? Datestarted { get; set; }
        public DateTime? Datefinished { get; set; }
        public string Addedby { get; set; }
        public string Encounter_Id { get; set; }
        public bool? Isamended { get; set; }
        public decimal? Observationfrequency { get; set; }
        public string Observationscaletype_Id { get; set; }
        public bool? Escalationofcare { get; set; }
        public string Reasonforamend { get; set; }
        public string Reasonfordelete { get; set; }
        public string Reasonforincompleteobservations { get; set; }
        public string Eventcorrelationid { get; set; }
    }
}


