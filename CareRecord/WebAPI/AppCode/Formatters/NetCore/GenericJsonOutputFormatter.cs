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
﻿using Interneuron.CareRecord.HL7SynapseHandler.Service.Models;
using Interneuron.Infrastructure.CustomExceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Interneuron.CareRecord.API.AppCode.Formatters.NetCore
{
    public class GenericJsonOutputFormatter : TextOutputFormatter
    {
        public GenericJsonOutputFormatter()
        {
            SupportedEncodings.Clear();
            SupportedEncodings.Add(Encoding.UTF8);

            SupportedMediaTypes.Add("application/json");
            SupportedMediaTypes.Add("text/json");
        }

        protected override bool CanWriteType(Type type)
        {
            return !(typeof(SynapseResource).IsAssignableFrom(type) || typeof(List<SynapseResource>).IsAssignableFrom(type)) && !(typeof(Hl7.Fhir.Model.Resource).IsAssignableFrom(type));
        }

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (selectedEncoding == null) throw new ArgumentNullException(nameof(selectedEncoding));

            if (selectedEncoding != Encoding.UTF8)
                throw new InterneuronBusinessException((short)StatusCodes.Status400BadRequest, $"API supports UTF-8 encoding exclusively, not {selectedEncoding.WebName}");

            context.HttpContext.Response.StatusCode = StatusCodes.Status200OK;// (int)response.StatusCode;
            var apiResponse = context.Object;

            var responseString = JsonConvert.SerializeObject(apiResponse);

            return context.HttpContext.Response.WriteAsync(responseString);
        }
    }
}
