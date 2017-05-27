using System.Drawing;
using Game.BaseStructures;
using Game.BaseStructures.Enums;
using Game.Fighters;
using Game.GameObjects;

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
            CurrentImage = fighter.LookingRight ? Picture.Right : Picture.Left;
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
                fighter.LookingRight = true;
            }
            if (fighter.State == FighterMotionState.MovingLeft)
            {
                CurrentImage = Picture.GetMovingImage(movingRight: false);
                fighter.LookingRight = false;
            }
        }

        private void UpdateFighterBattleImage()
        {
            if (fighter.IsAttacking)
            {
                if (Trigger)
                    PreviousImage = CurrentImage;
                Trigger = false;
                CurrentImage = fighter.LookingRight ? Picture.AttackRight : Picture.AttackLeft;
            }
            else if (fighter.IsBlocking)
            {
                if (Trigger)
                    PreviousImage = CurrentImage;
                Trigger = false;
                CurrentImage = fighter.LookingRight ? Picture.BlockRight : Picture.BlockLeft;
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
