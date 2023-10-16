namespace SomeBlog.Model.Enums
{
    public enum FormItemType
    {
        Input = 1,
        TextArea = 2
    }

    public class FormItemTypeEnumHelpers
    {
        public static string ToFiendlyName(int value)
        {
            switch (value) {
                case 1: return "Input";
                case 2: return "TextArea";
                default: return value.ToString();
            }
        }
    }
}
