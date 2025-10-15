namespace CSTest.Session03.FunctionalRefactoringPrimitiveObsession;

interface IStorage<in T>
{
    void Flush(T item);
}