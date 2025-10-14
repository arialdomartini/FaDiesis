namespace CSTest.Session03.FunctionalRefactoring;

interface IStorage<in T>
{
    void Flush(T item);
}