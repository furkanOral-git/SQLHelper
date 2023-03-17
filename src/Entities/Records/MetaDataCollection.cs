namespace SQLHelper.Entities.Records
{
    
    internal record MetaDataCollection
    (string TableName, string[] TablePropertyNames, Type[]? EntityConstructorTypes);
       
    
}