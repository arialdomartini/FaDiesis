namespace CSTest.Session03.FunctionalRefactoringRecords;

interface IStorage<in T>
{
    void Flush(T item);
}