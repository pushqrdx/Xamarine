﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Xamarine.Hosting.Swan.Formatters;

namespace Xamarine.Hosting.Swan.Networking
{
    /// <summary>
    ///     DnsClient Response inner class.
    /// </summary>
    internal partial class DnsClient
    {
        public class DnsClientResponse : IDnsResponse
        {
            private readonly byte[] _message;
            private readonly DnsResponse _response;

            internal DnsClientResponse(DnsClientRequest request, DnsResponse response, byte[] message)
            {
                Request = request;

                _message = message;
                _response = response;
            }

            public DnsClientRequest Request { get; }

            public int Id
            {
                get => _response.Id;
                set { }
            }

            public IList<IDnsResourceRecord> AnswerRecords => _response.AnswerRecords;

            public IList<IDnsResourceRecord> AuthorityRecords => new ReadOnlyCollection<IDnsResourceRecord>(_response.AuthorityRecords);

            public IList<IDnsResourceRecord> AdditionalRecords => new ReadOnlyCollection<IDnsResourceRecord>(_response.AdditionalRecords);

            public bool IsRecursionAvailable
            {
                get => _response.IsRecursionAvailable;
                set { }
            }

            public bool IsAuthorativeServer
            {
                get => _response.IsAuthorativeServer;
                set { }
            }

            public bool IsTruncated
            {
                get => _response.IsTruncated;
                set { }
            }

            public DnsOperationCode OperationCode
            {
                get => _response.OperationCode;
                set { }
            }

            public DnsResponseCode ResponseCode
            {
                get => _response.ResponseCode;
                set { }
            }

            public IList<DnsQuestion> Questions => new ReadOnlyCollection<DnsQuestion>(_response.Questions);

            public int Size => _message.Length;

            public byte[] ToArray()
            {
                return _message;
            }

            public override string ToString()
            {
                return _response.ToString();
            }
        }

        public class DnsResponse : IDnsResponse
        {
            private DnsHeader _header;

            public DnsResponse(DnsHeader header, IList<DnsQuestion> questions, IList<IDnsResourceRecord> answers, IList<IDnsResourceRecord> authority, IList<IDnsResourceRecord> additional)
            {
                _header = header;
                Questions = questions;
                AnswerRecords = answers;
                AuthorityRecords = authority;
                AdditionalRecords = additional;
            }

            public IList<DnsQuestion> Questions { get; }

            public IList<IDnsResourceRecord> AnswerRecords { get; }

            public IList<IDnsResourceRecord> AuthorityRecords { get; }

            public IList<IDnsResourceRecord> AdditionalRecords { get; }

            public int Id
            {
                get => _header.Id;
                set => _header.Id = value;
            }

            public bool IsRecursionAvailable
            {
                get => _header.RecursionAvailable;
                set => _header.RecursionAvailable = value;
            }

            public bool IsAuthorativeServer
            {
                get => _header.AuthorativeServer;
                set => _header.AuthorativeServer = value;
            }

            public bool IsTruncated
            {
                get => _header.Truncated;
                set => _header.Truncated = value;
            }

            public DnsOperationCode OperationCode
            {
                get => _header.OperationCode;
                set => _header.OperationCode = value;
            }

            public DnsResponseCode ResponseCode
            {
                get => _header.ResponseCode;
                set => _header.ResponseCode = value;
            }

            public int Size => _header.Size + Questions.Sum(q => q.Size) + AnswerRecords.Sum(a => a.Size) + AuthorityRecords.Sum(a => a.Size) + AdditionalRecords.Sum(a => a.Size);

            public byte[] ToArray()
            {
                UpdateHeader();
                var result = new MemoryStream(Size);

                result.Append(_header.ToArray()).Append(Questions.Select(q => q.ToArray())).Append(AnswerRecords.Select(a => a.ToArray())).Append(AuthorityRecords.Select(a => a.ToArray())).Append(AdditionalRecords.Select(a => a.ToArray()));

                return result.ToArray();
            }

            public static DnsResponse FromArray(byte[] message)
            {
                var header = DnsHeader.FromArray(message);
                var offset = header.Size;

                if (!header.Response || header.QuestionCount == 0)
                    throw new ArgumentException("Invalid response message");

                if (header.Truncated)
                    return new DnsResponse(header, DnsQuestion.GetAllFromArray(message, offset, header.QuestionCount), new List<IDnsResourceRecord>(), new List<IDnsResourceRecord>(), new List<IDnsResourceRecord>());

                return new DnsResponse(header, DnsQuestion.GetAllFromArray(message, offset, header.QuestionCount, out offset), DnsResourceRecordFactory.GetAllFromArray(message, offset, header.AnswerRecordCount, out offset),
                                       DnsResourceRecordFactory.GetAllFromArray(message, offset, header.AuthorityRecordCount, out offset), DnsResourceRecordFactory.GetAllFromArray(message, offset, header.AdditionalRecordCount, out offset));
            }

            public override string ToString()
            {
                UpdateHeader();

                return Json.SerializeOnly(this, true, nameof(Questions), nameof(AnswerRecords), nameof(AuthorityRecords), nameof(AdditionalRecords));
            }

            private void UpdateHeader()
            {
                _header.QuestionCount = Questions.Count;
                _header.AnswerRecordCount = AnswerRecords.Count;
                _header.AuthorityRecordCount = AuthorityRecords.Count;
                _header.AdditionalRecordCount = AdditionalRecords.Count;
            }
        }
    }
}
