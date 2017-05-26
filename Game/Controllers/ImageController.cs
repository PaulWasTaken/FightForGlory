using System.Drawing;
using Game.BaseStructures;
using Game.BaseStructures.AbstractClasses;
using Game.BaseStructures.Enums;

namespace Game.Controllers
{
    public class ImageController
    {
        private readonly Fighter fighter;
        private ImageInfo Picture { get; set; }
        public Image CurrentImage { get; set; }
        private Image PreviousImage { get; set; }
        private bool Trigger { get; set; }

        public ImageController(Fighter fighter)
        {
            this.fighter = fighter;
            Trigger = true;
            Picture = ImageInfo.CreateFigtherInfo(fighter.Name);
            CurrentImage = fighter.LookRight ? Picture.Right : Picture.Left;
            PreviousImage = CurrentImage;
        }

        public ImageController()
        {
        }

        public Image GetCurrentObjImage(GameObject obj)
        {
            if (Picture == null)
                Picture = ImageInfo.CreateGameObjectInfo(obj.GetType().Name, new Size((int)obj.Size.Width, (int)obj.Size.Height));
            return obj.Source == PlayerNumber.FirstPlayer ? Picture.Right : Picture.Left;
        }

        private void UpdateFighterMovingImage()
        {
            if (fighter.State == FighterMotionState.MovingRight)
            {
                CurrentImage = Picture.GetMovingImage(movingRight: true);
                fighter.LookRight = true;
            }
            if (fighter.State == FighterMotionState.MovingLeft)
            {
                CurrentImage = Picture.GetMovingImage(movingRight: false);
                fighter.LookRight = false;
            }
        }

        private void UpdateFighterBattleImage()
        {
            if (fighter.Attack)
            {
                if (Trigger)
                    PreviousImage = CurrentImage;
                Trigger = false;
                CurrentImage = fighter.LookRight ? Picture.AttackRight : Picture.AttackLeft;
            }
            else if (fighter.Block.Blocking)
            {
                if (Trigger)
                    PreviousImage = CurrentImage;
                Trigger = false;
                CurrentImage = fighter.LookRight ? Picture.BlockRight : Picture.BlockLeft;
            }
            else
            {
                if (Trigger) return;
                CurrentImage = PreviousImage;
                Trigger = true;
            }
        }

        public void UpdateFighterImage()
        {
            UpdateFighterMovingImage();
            UpdateFighterBattleImage();
        }
    }
}
