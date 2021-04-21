namespace UnityTemplateProjects
{
    public static class Query
    {
        public const string DROP_MONSTERS_IF_EXISTS = @"DROP TABLE IF EXISTS monsters";

        public const string CREATE_TABLE_MONSTERS =
            @"create table if not exists monsters
                (
                    class             TEXT NOT NULL,
                    family            TEXT NOT NULL,
                    creature          TEXT NOT NULL,
                    trait             TEXT NOT NULL,
                    trait_description TEXT NOT NULL,
                    material          TEXT
                )";

        public const string INSERT_INTO_MONSTERS = 
            @"INSERT INTO monsters 
                (class, family, creature, trait, trait_description, material) 
                VALUES ($class, $family, $creature, $trait, $description, $material)";

        public const string SELECT_FROM_MONSTER = 
            @"SELECT * FROM monsters";

        public const string SELECT_FIELD_FROM_MONSTERS =
            @"SELECT {field} FROM monsters";
        
    }
}