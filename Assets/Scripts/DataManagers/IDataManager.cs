using System.Collections.Generic;

public interface IDataManager
{
    public void Insert(CreatureModel model);
    public IEnumerable<CreatureModel> QueryForCreatures(CreatureQueryModel queryModel);
    public List<string> QueryDistinctClass();
    public List<string> QueryDistinctFamily();
        
    public void Initialize(IEnumerable<CreatureModel> creatures);
}