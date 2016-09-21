using System;

namespace WorkTimeControllerEntities
{
    public class ActionEntity
    {
        public const string PlayActionText = "Início";
        public const string PauseActionText = "Pausa";
        public const string FinishActionText = "Encerrado";

        public string ActionText { get; set; }
        public TimeSpan ActionTime { get; set; }
        public ActionType Type { get; set; }

        public ActionEntity(ActionType actionType)
        {
            switch (actionType)
            {
                case ActionType.Start:
                    ActionText = PlayActionText;
                    break;
                case ActionType.Pause:
                    ActionText = PauseActionText;
                    break;
                case ActionType.Finish:
                    ActionText = FinishActionText;
                    break;
                default:
                    break;
            }

            Type = actionType;
        }
    }

    public enum ActionType
    {
        Start,
        Pause,
        Finish
    }
}
