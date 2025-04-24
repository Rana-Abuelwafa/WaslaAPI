﻿using WaslaApp.Data;
using WaslaApp.Data.Entities;
using WaslaApp.Data.Models;

namespace Wasla_App.services
{
    public class WaslaService : IWaslaService
    {
        private WaslaDAO _waslaDao;

        public WaslaService(WaslaDAO waslaDao)
        {
            _waslaDao = waslaDao;

        }

        public ResponseCls deleteQuestions(RegistrationQuestion ques)
        {
            return _waslaDao.deleteQuestions(ques);
        }

        public Task<List<ClientBrand>> GetClientBrands(string clientId)
        {
            return _waslaDao.GetClientBrands(clientId);
        }

        public Task<List<ClientImage>> GetProfileImage(string clientId)
        {
            return _waslaDao.GetProfileImage(clientId);
        }

        public Task<List<ClientProfile>> GetClientProfiles(string clientId)
        {
            return _waslaDao.GetClientProfiles(clientId);
        }

        public Task<List<PaymentMethod>> GetPaymentMethods()
        {
            return _waslaDao.GetPaymentMethods();
        }

        public Task<List<RegistrationQuestion>> getRegistrationQuestionList(string lang)
        {
            return _waslaDao.getRegistrationQuestionList(lang);
        }

        public ResponseCls saveClientBrand(ClientBrand brand)
        {
            return _waslaDao.saveClientBrand(brand);
        }

        public ResponseCls saveProfileImage(ClientImage image)
        {
            return _waslaDao.saveProfileImage(image).Result;
        }

        public ResponseCls saveMainProfile(ClientProfile profile)
        {
            return _waslaDao.saveMainProfile(profile);
        }

        public ResponseCls saveQuestions(RegistrationQuestion ques)
        {
            return _waslaDao.saveQuestions(ques);
        }

        public RegsistrationQuesResponse saveRegistrationSteps(List<RegistrationAnswer> lst,string clientId,string FullName,string email)
        {
            return _waslaDao.saveRegistrationSteps(lst, clientId, FullName, email);
        }

        public List<QuesWithAnswers> getQuesWithAnswers(string clientId, string lang)
        {
            return _waslaDao.getQuesWithAnswers(clientId, lang);
        }
    }
}
