using System.Text;

namespace CadeOFogo.Utilities
{
  public static class UrlClean
  {
    public static string PercentEncode(string value)  
    {  
      StringBuilder retval = new StringBuilder();  
      foreach (char c in value)  
      {   
        if ((c >= 48 && c <= 57) || //0-9  
            (c >= 65 && c <= 90) || //a-z  
            (c >= 97 && c <= 122) || //A-Z                    
            (c == 45 || c == 46 || c == 95 || c == 126)) // period, hyphen, underscore, tilde  
        {  
          retval.Append(c);  
        }  
        else  
        {  
          retval.AppendFormat("%{0:X2}", ((byte)c));  
        }  
      }  
      return retval.ToString();  
    } 
  }
}