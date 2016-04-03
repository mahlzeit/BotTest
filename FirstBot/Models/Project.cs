using Microsoft.Bot.Builder.FormFlow;
using System;

namespace FirstBot.Models
{
    public enum ApplicationType
    {
        [Describe("Apple iOS")]
        iOS,
        Android,
        [Describe("Apple iOS and Android")]
        CrossPlatform
    }

    public enum LoginType
    {
        Email,
        Social,
        No,
        [Describe("I don´t know")]
        Unknown
    }

    public enum MonetizationType
    {
        UpfrontCost,
        [Describe("In-app Purchases")]
        InAppPurchases,
        Free,
        [Describe("I don´t know")]
        Unknown
    }

    public enum DesignExcellence
    {
        [Describe("Bare-bones")]
        BareBones,
        Stock,
        Beautiful
    }

    [Serializable]
    [Template(TemplateUsage.EnumSelectOne, ChoiceStyle = ChoiceStyleOptions.PerLine)]
    [Template(TemplateUsage.NotUnderstood, "I do not understand \"{0}\".", "Try again, I don't get \"{0}\".")]
    class Project
    {
        [Prompt("What type of app are you building? {||}")]
        public ApplicationType? ApplicationType;

        [Prompt("Do people have to login? {||}")]
        public LoginType LoginRequired;

        [Prompt("Do people create personal profiles? {||}")]
        public bool? PersonalProfileUsed;

        [Prompt("How will you make money from your app? {||}")]
        public MonetizationType MonetizationType;

        [Prompt("Do people rate or review things? {||}")]
        public bool? WithReview;

        [Prompt("Does your app need to connect with your website? {||}")]
        public bool? ConnectsToWebsite;

        [Prompt("How nice should your app look? {||}")]
        public DesignExcellence? DesignExcellence;

        [Prompt("Do you need an app icon? {||}")]
        public bool? AppIconNeeded;

        public static IForm<Project> BuildForm()
        {
            return new FormBuilder<Project>()
                .Message("Welcome to the project budget planner bot!")
                .Field(nameof(ApplicationType))
                .Field(nameof(LoginRequired))
                .Field(nameof(PersonalProfileUsed))
                .Field(nameof(MonetizationType))
                .Field(nameof(WithReview))
                .Field(nameof(ConnectsToWebsite))
                .Field(nameof(DesignExcellence))
                .Field(nameof(AppIconNeeded))
                .Build();
        }

        private static int CalculatePrice(Project project)
        {
            int price = 0;

            switch (project.ApplicationType)
            {
                case Models.ApplicationType.Android | Models.ApplicationType.iOS:
                    price += 8000;
                    break;
                case Models.ApplicationType.CrossPlatform:
                    price += 16000;
                    break;
                default:
                    break;
            }

            switch(project.LoginRequired)
            {
                case LoginType.Email:
                    price += 2000;
                    break;
                case LoginType.Social:
                    price += 4000;
                    break;
                default:
                    break;
            }

            if (project.PersonalProfileUsed ?? false)
            {
                price += 3000;
            }

            switch(project.MonetizationType)
            {
                case MonetizationType.UpfrontCost:
                    price += 1000;
                    break;
                case MonetizationType.InAppPurchases:
                    price += 3000;
                    break;
                default:
                    break;
            }

            if (project.WithReview ?? false)
            {
                price += 3000;
            }

            if (project.ConnectsToWebsite ?? false)
            {
                price += 5000;
            }

            switch(project.DesignExcellence)
            {
                case Models.DesignExcellence.BareBones:
                    price += 2000;
                    break;
                case Models.DesignExcellence.Stock:
                    price += 4000;
                    break;
                case Models.DesignExcellence.Beautiful:
                    price += 10000;
                    break;
                default:
                    break;
            }

            if (project.AppIconNeeded ?? false)
            {
                price += 4000;
            }

            return price;
        }
    }
}
