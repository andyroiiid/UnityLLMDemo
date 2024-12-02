using System.Text;

public static class StringBuilderExtensions
{
    public static void AppendLlama3Dialog(this StringBuilder sb, string role, string content)
    {
        sb.Append("<|start_header_id|>");
        sb.Append(role);
        sb.Append("<|end_header_id|>");
        sb.Append(content);
        sb.Append("<|eot_id|>");
    }
}