﻿using Azure.Communication.Email;
using CourierHub.Shared.ApiModels;

namespace CourierHub.Server.Data {
    public class ContentCreator {
        private readonly ApiService _service; // podejrzewam, że się przyda

        public ContentCreator(ApiService service) {
            _service = service;
        }

        public EmailContent CreateMailContent(ApiMailContent content) {
            // tu trzeba stworzyć zawartość maila z linkiem
            // coś dla ciebie Maks
            throw new NotImplementedException();
        }

        public string CreateContract(ApiContract contract) {
            // tu trzeba stworzyć .txt z umową
            // jak będzie trzeba przerobimy na pdf i potem na bajty
            throw new NotImplementedException();
        }

        public string CreateReceipt(ApiReceipt receipt) {
            // tu trzeba stworzyć .txt z paragon
            // jak będzie trzeba przerobimy na pdf i potem na bajty
            throw new NotImplementedException();
        }
    }
}
