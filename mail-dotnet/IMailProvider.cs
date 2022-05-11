
namespace mail_dotnet {
    public interface IMailProvider {
        public bool TestProvider() {
            return TestProvider(out _);
        }
        public bool TestProvider(out string result);
    }
}
