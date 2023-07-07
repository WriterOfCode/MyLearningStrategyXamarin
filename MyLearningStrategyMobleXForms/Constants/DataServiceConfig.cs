using System;
using System.Collections.Generic;
using System.Text;

namespace MyLearningStrategyMobleXForms.Services
{
    public static class DataServiceConfig
    {
        public const int Retries = 5;
        public const int FromSeconds = 3;
        public const int RetryMultiplyer = 3;
        public const int CacheExpireSec = 60;
        public const int CacheExpireMin = 0;
        public const int CacheExpireHour = 120;
        public const int CacheExpireDays = 0;
               
        public const string ApplicationName = "MyLearningStrategyMobleXForms";
        public const string Categories = "api/Categories/";
        public const string LearningHistory = "api/LearningHistory/";
        public const string LearningHistoryProgress = "api/LearningHistoryProgress/";
        public const string Questions = "api/Questions/";
        public const string QuestionsCategories = "api/QuestionsCategories/";
        public const string Responses = "api/Responses/";
        public const string Strategies = "api/Strategies/";
        public const string Subjects = "api/Subjects/";
        public const string SubjectsCategories = "api/SubjectsCategories/";
        public const string UserPermissions = "api/UserPermissions/";
        public const string UserProfiles = "api/UserProfiles/";
        public const string Stats = "api/Stats/";
        //public const string ApiBaseUrl= "https://localhost:5001/";
        //public const string ApiBaseUrl = "https://mylearningstrategywebapi.azurewebsites.net/";
        public const string ApiBaseUrl = "https://mylearningstrategywebapidev.azurewebsites.net/";
        //public const string ApiBaseUrl = "https://localhost/";
    }
}

