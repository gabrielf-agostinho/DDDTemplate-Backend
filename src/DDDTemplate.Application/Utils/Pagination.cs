namespace DDDTemplate.Application.Utils;

public class Pagination<T>(int total, IEnumerable<T> list)
{
  public int Total { get; set; } = total;
  public IEnumerable<T> List { get; set; } = list;
}
