using System;
using System.Net.Sockets;
using System.Text;
using Backups.Entities;
using Backups.Tools;
using BackupsExtra.Entities;

namespace Backups.Client.Entities
{
    public class BackupsTcpClient
    {
        private BackupJoba _backupJoba;
        private JsonStringBuilder _jsonStringBuilder;
        private BackupJobaJsonRepresenter _jobaJsonRepresenter;
        private string _ip;
        private int _port;
        public BackupsTcpClient(string ip, int port, BackupJoba backupJoba)
        {
            _backupJoba = backupJoba;
            _jsonStringBuilder = new JsonStringBuilder();
            _jobaJsonRepresenter = new BackupJobaJsonRepresenter(backupJoba);
            _ip = ip;
            _port = port;
        }

        public void SendConfiguration()
        {
            _jsonStringBuilder.Clear();
            _jsonStringBuilder.AddProperty("MessageType", "Configuration");
            _jsonStringBuilder.AddProperty(
                "BackupJoba",
                Encoding.Default.GetString(_jobaJsonRepresenter.GetConfigurationRepresentation()));

            SendData(_jsonStringBuilder.GetResult());
        }

        public void SendAddJobObject(IJobObject jobObject)
        {
            _jsonStringBuilder.Clear();
            string name = jobObject.Name;
            string extension = jobObject.Extension;
            string representation = Encoding.Default.GetString(jobObject.GetRepresentation());

            _jsonStringBuilder.AddProperty("MessageType", "AddObject");
            _jsonStringBuilder.AddProperty("Name", name);
            _jsonStringBuilder.AddProperty("Extension", extension);

            string resultJson = _jsonStringBuilder.GetResult();
            SendData(resultJson, representation.ToString());
        }

        public void SendRemoveJobObject(IJobObject jobObject)
        {
            _jsonStringBuilder.Clear();
            string name = jobObject.Name;
            string extension = jobObject.Extension;
            string representation = Encoding.Default.GetString(jobObject.GetRepresentation());

            _jsonStringBuilder.AddProperty("MessageType", "RemoveObject");
            _jsonStringBuilder.AddProperty("Name", name);
            _jsonStringBuilder.AddProperty("Extension", extension);
            _jsonStringBuilder.AddProperty("Representation", representation);

            SendData(_jsonStringBuilder.GetResult());
        }

        public void SendProcessObjects()
        {
            _jsonStringBuilder.Clear();
            _jsonStringBuilder.AddProperty("MessageType", "ProcessObjects");
            SendData(_jsonStringBuilder.GetResult());
        }

        private void SendData(string jsonString, string fileRepresentation = "")
        {
            var tcpClient = new TcpClient();
            tcpClient.Connect(_ip, _port);
            byte[] jsonSize = BitConverter.GetBytes(jsonString.Length);
            byte[] jsonData = Encoding.Default.GetBytes(jsonString);
            byte[] fileSize = BitConverter.GetBytes(fileRepresentation.Length);
            byte[] fileData = Encoding.Default.GetBytes(fileRepresentation);
            NetworkStream dataStream = tcpClient.GetStream();

            dataStream.Write(jsonSize, 0, jsonSize.Length);
            dataStream.Write(jsonData, 0, jsonData.Length);
            dataStream.Write(fileSize, 0, fileSize.Length);
            dataStream.Write(fileData, 0, fileData.Length);
            dataStream.Close();
            tcpClient.Close();
        }
    }
}
