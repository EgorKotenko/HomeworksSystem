using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace diploma1
{

  public class Courses
  {
    public string Id { get; set; }
    public string Name { get; set; }
    public bool Selected { get; set; }
  }


  public class Homeworks
  {
    public string Id { get; set; }
    public DateTime Date { get; set; }
    public string CourseID { get; set; }
    public int Number { get; set; }
    public string Name { get; set; }
  }


  public class Problems
  {
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; }
    public string Statement { get; set; }   //  byte[] -> BASE64 -> byte[]
    public string Answer { get; set; }
    public string HomeworkID { get; set; }
    public int Number { get; set; }
    public bool? Completed { get; set; }
  }

}
