using UnityEngine;

public class FieldsFactory : MonoBehaviour
{
    [SerializeField]
    private GameObject pattern;

    [SerializeField]
    private Level level;

    public Field[,] Create(int[,] fieldTypes)
    {
        Field[,] fields = new Field[fieldTypes.GetLength(0), fieldTypes.GetLength(1)];

        for (int y = 0; y < fieldTypes.GetLength(0); y++)
        {
            for (int x = 0; x < fieldTypes.GetLength(1); x++)
            {
                GameObject instance = Instantiate(pattern, level.gameObject.transform) as GameObject;
                Field field = instance.GetComponent<Field>();
                field.Init(x, y, fieldTypes[y, x]);
                    
                fields[y, x] = field;
            }
        }

        return fields;
    }
}
