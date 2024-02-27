namespace DC_ARPG
{
    public interface IDataPersistence
    {
        public string PrefabId { get; }
        public string EntityId { get; }
        public bool IsCreated { get; }

        public bool IsSerializable();
        public string SerializeState();
        public void DeserializeState(string state);

        public void SetupCreatedDataPersistenceObject(string entityId, bool isCreated, string state);
    }
}
