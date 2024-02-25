namespace DC_ARPG
{
    public interface IDataPersistence
    {
        public long EntityId { get; }

        public bool IsSerializable();
        public string SerializeState();
        public void DeserializeState(string state);
    }
}
