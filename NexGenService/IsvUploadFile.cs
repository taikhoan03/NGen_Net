using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace NexGenService
{
    [ServiceContract]
    public interface IsvUploadFile
    {
        [OperationContract]
        //[TransactionFlow(TransactionFlowOption.Allowed)]
        RemoteFileInfo DownloadFile(DownloadRequest request);

        [OperationContract]
        void UploadFile(RemoteFileInfo request);
    }

    [MessageContract]
    public class DownloadRequest
    {
        [MessageBodyMember]
        public string FileName;
        [MessageHeader(MustUnderstand = true)]
        public string FolderName;
        [MessageHeader(MustUnderstand = true)]
        public EnumType.FolderTypes FolderType;
    }

    [MessageContract]
    public class RemoteFileInfo : IDisposable
    {
        [MessageHeader(MustUnderstand = true)]
        public string FolderName;

        [MessageHeader(MustUnderstand = true)]
        public string FileName;

        [MessageHeader(MustUnderstand = true)]
        public EnumType.FolderTypes FolderType;

        [MessageHeader(MustUnderstand = true)]
        public long Length;

        [MessageBodyMember(Order = 1)]
        //[MessageHeader(MustUnderstand = true)]
        public System.IO.Stream FileByteStream;

        public void Dispose()
        {
            //if (FileByteStream != null)
            //{
            //    FileByteStream.Close();
            //    FileByteStream = null;
            //}
        }
    }

    [DataContract]
    
    public class EnumType
    {
        /// <summary>
        /// Enum: FolderTypes
        /// </summary>
        [DataContract(Name = "FolderTypes")]
        [Flags]
        public enum FolderTypes
        {
            // financial & accounting
            [EnumMember]
            Undefine = 0,
            [EnumMember]
            IMG,
            [EnumMember]
            OCR,
        }
    }
}
