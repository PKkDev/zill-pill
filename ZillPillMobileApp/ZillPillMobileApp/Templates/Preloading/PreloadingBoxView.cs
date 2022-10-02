namespace ZillPillMobileApp.Templates.Preloading
{
    public class PreloadingBoxView : BoxView
    {
        public PreloadingBoxView()
        {
            BackgroundColor = Color.FromHex("#95a5a6");
            CornerRadius = 6;

            var parentAnimation = new Animation();
            var animationTo = new Animation(v => Opacity = v, 1, 0.5);
            var animationFrom = new Animation(v => Opacity = v, 0.5, 1);

            parentAnimation.Add(0, 0.5, animationTo);
            parentAnimation.Add(0.5, 1, animationFrom);

            parentAnimation.Commit(this, "SpecFadeTo", 16, 1000, Easing.Linear, null, () => true);
        }
    }
}
