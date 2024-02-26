namespace DC_ARPG
{
    public interface IDataPersistence
    {
        public string EntityId { get; }

        public bool IsSerializable();
        public string SerializeState();
        public void DeserializeState(string state);
    }
}
