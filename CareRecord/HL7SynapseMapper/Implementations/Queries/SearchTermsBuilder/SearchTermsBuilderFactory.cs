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

namespace Interneuron.CareRecord.HL7SynapseHandler.Service.Implementations.Queries.SearchTermsBuilder
{
    public class SearchTermsBuilderFactory
    {
        private IServiceProvider _provider;

        public SearchTermsBuilderFactory()
        {

        }
        public SearchTermsBuilderFactory(IServiceProvider _provider)
        {
            this._provider = _provider;
        }

        public SearchTermsBuilder GetSearchTermsBuilder(string searchEntityIdentifier)
        {
            switch (searchEntityIdentifier.ToLower())
            {
                case "search_encounter":
                    return this._provider.GetService(typeof(EncounterSearchTermsBuilder)) as EncounterSearchTermsBuilder;
                case "search_observation":
                    return this._provider.GetService(typeof(ObservationResultSearchTermsBuilder)) as ObservationResultSearchTermsBuilder;
                case "search_patient":
                    return this._provider.GetService(typeof(PatientSearchTermsBuilder)) as PatientSearchTermsBuilder;
            }
            return null;
        }
    }
}


