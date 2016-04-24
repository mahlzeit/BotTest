using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.Luis;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FirstBot.Models
{
    [LuisModel("56936b40-ae07-450f-8368-a6ac38d2f61d", "d39eda09843b4b9cba15d25357fdbe83")]
    [Serializable]
    public class LuisDialog : LuisDialog<object>
    {
        internal static IFormDialog<Project> MakeRootDialog()
        {
            return FormDialog.FromForm(Project.BuildForm, FormOptions.PromptInStart);
        }

        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            string returnMessage = $"Sorry I did not understand: " + string.Join(", ", result.Intents.Select(i => i.Intent));
            await context.PostAsync(returnMessage);
            context.Wait(MessageReceived);
        }

        [LuisIntent("Greeting")]
        public async Task Greeting(IDialogContext context, LuisResult result)
        {
            string returnMessage = $"Hi, nice to meet you!";
            await context.PostAsync(returnMessage);
            context.Wait(MessageReceived);
        }

        [LuisIntent("getInfo")]
        public async Task GetInfo(IDialogContext context, LuisResult result)
        {
            if (result.Entities.Count == 0)
            {
                string returnMessage = $"Sorry I did not understand: " + string.Join(", ", result.Intents.Select(i => i.Intent));
                await context.PostAsync(returnMessage);
                context.Wait(MessageReceived);
            }

            var outcome = result.Entities.Where(e => e.Type.ToLower().Equals("outcome")).First().Entity;
            var projectType = result.Entities.Where(e => e.Type.ToLower().Equals("projecttype")).First().Entity;

            if (outcome.ToLower().Equals("cost estimate") || outcome.ToLower().Equals("estimate"))
            {
                if (projectType.ToLower().Equals("app"))
                {
                    await context.PostAsync("Sure, let´s go then. I'll hand you over to our project budget planner bot... His name is \"2PB\" (hint: He's not that good in interracial communication and preferres rather concrete answers ;) )");

                    IFormDialog<Project> formDialog = MakeRootDialog();
                    context.Call(formDialog, EstimationCompleted);
                }
                else
                {
                    await context.PostAsync("A cost estimate? Sure. But for what was it?");
                    context.Wait(MessageReceived);
                }
            }
            else
            {
                await context.PostAsync("Didn´t understand it quite yet. sry");
                context.Wait(MessageReceived);
            }
        }

        private async Task EstimationCompleted(IDialogContext context, IAwaitable<Project> result)
        {
            await context.PostAsync($"Ok, we did it...");
            context.Wait(MessageReceived);
        }
    }
}
