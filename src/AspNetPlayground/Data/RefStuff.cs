namespace AspNetPlayground.Data;


public class RefContainer
{
    private int[] data;

    public RefContainer(int size)
    {
        data = new int[size];
        for (int i = 0; i < size; i++)
        {
            data[i] = i + 1;
        }
    }

    public ref int this[int index] => ref data[index];

    public ref int GetRef(int index)
    {
        return ref data[index];
    }

    public override string ToString() => string.Join(", ", data);
}