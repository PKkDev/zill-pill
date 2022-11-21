namespace ZillPillMobileApp.Behaviors.EntryBeh
{
    public class EntryLengthValidator : Behavior<Entry>
    {
        // public int MaxLength { get; set; }
        public int MinLength { get; set; } = 0;

        protected override void OnAttachedTo(Entry bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.TextChanged += OnEntryTextChanged;
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.TextChanged -= OnEntryTextChanged;
        }

        void OnEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            Entry entry = (Entry)sender;

            var maxLength = entry.Text.StartsWith("+") ? 12 : 11;

            if (entry.Text.Length > maxLength)
            {
                string entryText = entry.Text;

                entryText = entryText.Remove(entryText.Length - 1);

                entry.Text = entryText;
            }

            if (MinLength > 0)
                if (entry.Text.Length < this.MinLength)
                {
                    ((Entry)sender).TextColor = Colors.Red;
                }
                else
                    ((Entry)sender).TextColor = Colors.Black;
        }
    }
}
