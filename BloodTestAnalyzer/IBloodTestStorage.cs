// Interface som definierar lagringen av provsvar

namespace BloodTestAnalyzer
{
    public interface IBloodTestStorage
    {
        List<TestResult> Load();

        void Save(List<TestResult> results);
    }
}