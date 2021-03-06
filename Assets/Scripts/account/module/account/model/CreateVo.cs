//// Generated by ProtoGen, Version=2.4.1.521, Culture=neutral, PublicKeyToken=55f7125234beb589.  DO NOT EDIT!
//#pragma warning disable 1591, 0612, 3021
//#region Designer generated code

//using pb = global::Google.ProtocolBuffers;
//using pbc = global::Google.ProtocolBuffers.Collections;
//using pbd = global::Google.ProtocolBuffers.Descriptors;
//using scg = global::System.Collections.Generic;
using ProtoBuf;
namespace com.tianhe.account.module.account.model
{

    //  [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [ProtoContract]
    public class CreateVo
    {
        public CreateVo() { }
        public CreateVo(string name, string password, int operator_, bool adult, int channel)
        {
            this.name = name;
            this.password = password;
            this.operator_ = operator_;
            this.adult = adult;
            this.channer = channel;
        }
        //    #region Extension registration
        //    public static void RegisterAllExtensions(pb::ExtensionRegistry registry) {
        //    }
        //    #endregion
        //    #region Static variables
        //    internal static pbd::MessageDescriptor internal__static_com_tianhe_account_module_account_model_CreateVo__Descriptor;
        //    internal static pb::FieldAccess.FieldAccessorTable<global::com.tianhe.account.module.account.model.CreateVo, global::com.tianhe.account.module.account.model.CreateVo.Builder> internal__static_com_tianhe_account_module_account_model_CreateVo__FieldAccessorTable;
        //    internal static pbd::MessageDescriptor internal__static_com_tianhe_account_module_account_model_CreateResult__Descriptor;
        //    internal static pb::FieldAccess.FieldAccessorTable<global::com.tianhe.account.module.account.model.CreateResult, global::com.tianhe.account.module.account.model.CreateResult.Builder> internal__static_com_tianhe_account_module_account_model_CreateResult__FieldAccessorTable;
        //    #endregion
        //    #region Descriptor
        //    public static pbd::FileDescriptor Descriptor {
        //      get { return descriptor; }
        //    }
        //    private static pbd::FileDescriptor descriptor;

        //    static CreateModel() {
        //      byte[] descriptorData = global::System.Convert.FromBase64String(
        //          "ChFDcmVhdGVNb2RlbC5wcm90bxInY29tLnRpYW5oZS5hY2NvdW50Lm1vZHVs" + 
        //          "ZS5hY2NvdW50Lm1vZGVsIlwKCENyZWF0ZVZvEgwKBG5hbWUYASACKAkSEAoI" + 
        //          "cGFzc3dvcmQYAiACKAkSEAoIb3BlcmF0b3IYAyACKAUSDQoFYWR1bHQYBCAB" + 
        //          "KAgSDwoHY2hhbm5lbBgFIAEoBSIcCgxDcmVhdGVSZXN1bHQSDAoEY29kZRgB" + 
        //          "IAIoBQ==");
        //      pbd::FileDescriptor.InternalDescriptorAssigner assigner = delegate(pbd::FileDescriptor root) {
        //        descriptor = root;
        //        internal__static_com_tianhe_account_module_account_model_CreateVo__Descriptor = Descriptor.MessageTypes[0];
        //        internal__static_com_tianhe_account_module_account_model_CreateVo__FieldAccessorTable = 
        //            new pb::FieldAccess.FieldAccessorTable<global::com.tianhe.account.module.account.model.CreateVo, global::com.tianhe.account.module.account.model.CreateVo.Builder>(internal__static_com_tianhe_account_module_account_model_CreateVo__Descriptor,
        //                new string[] { "Name", "Password", "Operator", "Adult", "Channel", });
        //        internal__static_com_tianhe_account_module_account_model_CreateResult__Descriptor = Descriptor.MessageTypes[1];
        //        internal__static_com_tianhe_account_module_account_model_CreateResult__FieldAccessorTable = 
        //            new pb::FieldAccess.FieldAccessorTable<global::com.tianhe.account.module.account.model.CreateResult, global::com.tianhe.account.module.account.model.CreateResult.Builder>(internal__static_com_tianhe_account_module_account_model_CreateResult__Descriptor,
        //                new string[] { "Code", });
        //        return null;
        //      };
        //      pbd::FileDescriptor.InternalBuildGeneratedFileFrom(descriptorData,
        //          new pbd::FileDescriptor[] {
        //          }, assigner);
        //    }
        //    #endregion

        //  }
        //  #region Messages
        //  [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        //  public sealed partial class CreateVo : pb::GeneratedMessage<CreateVo, CreateVo.Builder> {
        //    private CreateVo() { }
        //    private static readonly CreateVo defaultInstance = new CreateVo().MakeReadOnly();
        //    private static readonly string[] _createVoFieldNames = new string[] { "adult", "channel", "name", "operator", "password" };
        //    private static readonly uint[] _createVoFieldTags = new uint[] { 32, 40, 10, 24, 18 };
        //    public static CreateVo DefaultInstance {
        //      get { return defaultInstance; }
        //    }

        //    public override CreateVo DefaultInstanceForType {
        //      get { return DefaultInstance; }
        //    }

        //    protected override CreateVo ThisMessage {
        //      get { return this; }
        //    }

        //    public static pbd::MessageDescriptor Descriptor {
        //      get { return global::com.tianhe.account.module.account.model.CreateModel.internal__static_com_tianhe_account_module_account_model_CreateVo__Descriptor; }
        //    }

        //    protected override pb::FieldAccess.FieldAccessorTable<CreateVo, CreateVo.Builder> InternalFieldAccessors {
        //      get { return global::com.tianhe.account.module.account.model.CreateModel.internal__static_com_tianhe_account_module_account_model_CreateVo__FieldAccessorTable; }
        //    }

        //    public const int NameFieldNumber = 1;
        //    private bool hasName;
        //    private string name_ = "";
        //    public bool HasName {
        //      get { return hasName; }
        //    }
        //    public string Name {
        //      get { return name_; }
        //    }

        //    public const int PasswordFieldNumber = 2;
        //    private bool hasPassword;
        //    private string password_ = "";
        //    public bool HasPassword {
        //      get { return hasPassword; }
        //    }
        //    public string Password {
        //      get { return password_; }
        //    }

        //    public const int OperatorFieldNumber = 3;
        //    private bool hasOperator;
        //    private int operator_;
        //    public bool HasOperator {
        //      get { return hasOperator; }
        //    }
        //    public int Operator {
        //      get { return operator_; }
        //    }

        //    public const int AdultFieldNumber = 4;
        //    private bool hasAdult;
        //    private bool adult_;
        //    public bool HasAdult {
        //      get { return hasAdult; }
        //    }
        //    public bool Adult {
        //      get { return adult_; }
        //    }

        //    public const int ChannelFieldNumber = 5;
        //    private bool hasChannel;
        //    private int channel_;
        //    public bool HasChannel {
        //      get { return hasChannel; }
        //    }
        //    public int Channel {
        //      get { return channel_; }
        //    }

        //    public override bool IsInitialized {
        //      get {
        //        if (!hasName) return false;
        //        if (!hasPassword) return false;
        //        if (!hasOperator) return false;
        //        return true;
        //      }
        //    }

        //    public override void WriteTo(pb::ICodedOutputStream output) {
        //      int size = SerializedSize;
        //      string[] field_names = _createVoFieldNames;
        //      if (hasName) {
        //        output.WriteString(1, field_names[2], Name);
        //      }
        //      if (hasPassword) {
        //        output.WriteString(2, field_names[4], Password);
        //      }
        //      if (hasOperator) {
        //        output.WriteInt32(3, field_names[3], Operator);
        //      }
        //      if (hasAdult) {
        //        output.WriteBool(4, field_names[0], Adult);
        //      }
        //      if (hasChannel) {
        //        output.WriteInt32(5, field_names[1], Channel);
        //      }
        //      UnknownFields.WriteTo(output);
        //    }

        //    private int memoizedSerializedSize = -1;
        //    public override int SerializedSize {
        //      get {
        //        int size = memoizedSerializedSize;
        //        if (size != -1) return size;

        //        size = 0;
        //        if (hasName) {
        //          size += pb::CodedOutputStream.ComputeStringSize(1, Name);
        //        }
        //        if (hasPassword) {
        //          size += pb::CodedOutputStream.ComputeStringSize(2, Password);
        //        }
        //        if (hasOperator) {
        //          size += pb::CodedOutputStream.ComputeInt32Size(3, Operator);
        //        }
        //        if (hasAdult) {
        //          size += pb::CodedOutputStream.ComputeBoolSize(4, Adult);
        //        }
        //        if (hasChannel) {
        //          size += pb::CodedOutputStream.ComputeInt32Size(5, Channel);
        //        }
        //        size += UnknownFields.SerializedSize;
        //        memoizedSerializedSize = size;
        //        return size;
        //      }
        //    }

        //    public static CreateVo ParseFrom(pb::ByteString data) {
        //      return ((Builder) CreateBuilder().MergeFrom(data)).BuildParsed();
        //    }
        //    public static CreateVo ParseFrom(pb::ByteString data, pb::ExtensionRegistry extensionRegistry) {
        //      return ((Builder) CreateBuilder().MergeFrom(data, extensionRegistry)).BuildParsed();
        //    }
        //    public static CreateVo ParseFrom(byte[] data) {
        //      return ((Builder) CreateBuilder().MergeFrom(data)).BuildParsed();
        //    }
        //    public static CreateVo ParseFrom(byte[] data, pb::ExtensionRegistry extensionRegistry) {
        //      return ((Builder) CreateBuilder().MergeFrom(data, extensionRegistry)).BuildParsed();
        //    }
        //    public static CreateVo ParseFrom(global::System.IO.Stream input) {
        //      return ((Builder) CreateBuilder().MergeFrom(input)).BuildParsed();
        //    }
        //    public static CreateVo ParseFrom(global::System.IO.Stream input, pb::ExtensionRegistry extensionRegistry) {
        //      return ((Builder) CreateBuilder().MergeFrom(input, extensionRegistry)).BuildParsed();
        //    }
        //    public static CreateVo ParseDelimitedFrom(global::System.IO.Stream input) {
        //      return CreateBuilder().MergeDelimitedFrom(input).BuildParsed();
        //    }
        //    public static CreateVo ParseDelimitedFrom(global::System.IO.Stream input, pb::ExtensionRegistry extensionRegistry) {
        //      return CreateBuilder().MergeDelimitedFrom(input, extensionRegistry).BuildParsed();
        //    }
        //    public static CreateVo ParseFrom(pb::ICodedInputStream input) {
        //      return ((Builder) CreateBuilder().MergeFrom(input)).BuildParsed();
        //    }
        //    public static CreateVo ParseFrom(pb::ICodedInputStream input, pb::ExtensionRegistry extensionRegistry) {
        //      return ((Builder) CreateBuilder().MergeFrom(input, extensionRegistry)).BuildParsed();
        //    }
        //    private CreateVo MakeReadOnly() {
        //      return this;
        //    }

        //    public static Builder CreateBuilder() { return new Builder(); }
        //    public override Builder ToBuilder() { return CreateBuilder(this); }
        //    public override Builder CreateBuilderForType() { return new Builder(); }
        //    public static Builder CreateBuilder(CreateVo prototype) {
        //      return new Builder(prototype);
        //    }

        //    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        //    public sealed partial class Builder : pb::GeneratedBuilder<CreateVo, Builder> {
        //      protected override Builder ThisBuilder {
        //        get { return this; }
        //      }
        //      public Builder() {
        //        result = DefaultInstance;
        //        resultIsReadOnly = true;
        //      }
        //      internal Builder(CreateVo cloneFrom) {
        //        result = cloneFrom;
        //        resultIsReadOnly = true;
        //      }

        //      private bool resultIsReadOnly;
        //      private CreateVo result;

        //      private CreateVo PrepareBuilder() {
        //        if (resultIsReadOnly) {
        //          CreateVo original = result;
        //          result = new CreateVo();
        //          resultIsReadOnly = false;
        //          MergeFrom(original);
        //        }
        //        return result;
        //      }

        //      public override bool IsInitialized {
        //        get { return result.IsInitialized; }
        //      }

        //      protected override CreateVo MessageBeingBuilt {
        //        get { return PrepareBuilder(); }
        //      }

        //      public override Builder Clear() {
        //        result = DefaultInstance;
        //        resultIsReadOnly = true;
        //        return this;
        //      }

        //      public override Builder Clone() {
        //        if (resultIsReadOnly) {
        //          return new Builder(result);
        //        } else {
        //          return new Builder().MergeFrom(result);
        //        }
        //      }

        //      public override pbd::MessageDescriptor DescriptorForType {
        //        get { return global::com.tianhe.account.module.account.model.CreateVo.Descriptor; }
        //      }

        //      public override CreateVo DefaultInstanceForType {
        //        get { return global::com.tianhe.account.module.account.model.CreateVo.DefaultInstance; }
        //      }

        //      public override CreateVo BuildPartial() {
        //        if (resultIsReadOnly) {
        //          return result;
        //        }
        //        resultIsReadOnly = true;
        //        return result.MakeReadOnly();
        //      }

        //      public override Builder MergeFrom(pb::IMessage other) {
        //        if (other is CreateVo) {
        //          return MergeFrom((CreateVo) other);
        //        } else {
        //          base.MergeFrom(other);
        //          return this;
        //        }
        //      }

        //      public override Builder MergeFrom(CreateVo other) {
        //        if (other == global::com.tianhe.account.module.account.model.CreateVo.DefaultInstance) return this;
        //        PrepareBuilder();
        //        if (other.HasName) {
        //          Name = other.Name;
        //        }
        //        if (other.HasPassword) {
        //          Password = other.Password;
        //        }
        //        if (other.HasOperator) {
        //          Operator = other.Operator;
        //        }
        //        if (other.HasAdult) {
        //          Adult = other.Adult;
        //        }
        //        if (other.HasChannel) {
        //          Channel = other.Channel;
        //        }
        //        this.MergeUnknownFields(other.UnknownFields);
        //        return this;
        //      }

        //      public override Builder MergeFrom(pb::ICodedInputStream input) {
        //        return MergeFrom(input, pb::ExtensionRegistry.Empty);
        //      }

        //      public override Builder MergeFrom(pb::ICodedInputStream input, pb::ExtensionRegistry extensionRegistry) {
        //        PrepareBuilder();
        //        pb::UnknownFieldSet.Builder unknownFields = null;
        //        uint tag;
        //        string field_name;
        //        while (input.ReadTag(out tag, out field_name)) {
        //          if(tag == 0 && field_name != null) {
        //            int field_ordinal = global::System.Array.BinarySearch(_createVoFieldNames, field_name, global::System.StringComparer.Ordinal);
        //            if(field_ordinal >= 0)
        //              tag = _createVoFieldTags[field_ordinal];
        //            else {
        //              if (unknownFields == null) {
        //                unknownFields = pb::UnknownFieldSet.CreateBuilder(this.UnknownFields);
        //              }
        //              ParseUnknownField(input, unknownFields, extensionRegistry, tag, field_name);
        //              continue;
        //            }
        //          }
        //          switch (tag) {
        //            case 0: {
        //              throw pb::InvalidProtocolBufferException.InvalidTag();
        //            }
        //            default: {
        //              if (pb::WireFormat.IsEndGroupTag(tag)) {
        //                if (unknownFields != null) {
        //                  this.UnknownFields = unknownFields.Build();
        //                }
        //                return this;
        //              }
        //              if (unknownFields == null) {
        //                unknownFields = pb::UnknownFieldSet.CreateBuilder(this.UnknownFields);
        //              }
        //              ParseUnknownField(input, unknownFields, extensionRegistry, tag, field_name);
        //              break;
        //            }
        //            case 10: {
        //              result.hasName = input.ReadString(ref result.name_);
        //              break;
        //            }
        //            case 18: {
        //              result.hasPassword = input.ReadString(ref result.password_);
        //              break;
        //            }
        //            case 24: {
        //              result.hasOperator = input.ReadInt32(ref result.operator_);
        //              break;
        //            }
        //            case 32: {
        //              result.hasAdult = input.ReadBool(ref result.adult_);
        //              break;
        //            }
        //            case 40: {
        //              result.hasChannel = input.ReadInt32(ref result.channel_);
        //              break;
        //            }
        //          }
        //        }

        //        if (unknownFields != null) {
        //          this.UnknownFields = unknownFields.Build();
        //        }
        //        return this;
        //      }


        //      public bool HasName {
        //        get { return result.hasName; }
        //      }
        //      public string Name {
        //        get { return result.Name; }
        //        set { SetName(value); }
        //      }
        //      public Builder SetName(string value) {
        //        pb::ThrowHelper.ThrowIfNull(value, "value");
        //        PrepareBuilder();
        //        result.hasName = true;
        //        result.name_ = value;
        //        return this;
        //      }
        //      public Builder ClearName() {
        //        PrepareBuilder();
        //        result.hasName = false;
        //        result.name_ = "";
        //        return this;
        //      }

        //      public bool HasPassword {
        //        get { return result.hasPassword; }
        //      }
        //      public string Password {
        //        get { return result.Password; }
        //        set { SetPassword(value); }
        //      }
        //      public Builder SetPassword(string value) {
        //        pb::ThrowHelper.ThrowIfNull(value, "value");
        //        PrepareBuilder();
        //        result.hasPassword = true;
        //        result.password_ = value;
        //        return this;
        //      }
        //      public Builder ClearPassword() {
        //        PrepareBuilder();
        //        result.hasPassword = false;
        //        result.password_ = "";
        //        return this;
        //      }

        //      public bool HasOperator {
        //        get { return result.hasOperator; }
        //      }
        //      public int Operator {
        //        get { return result.Operator; }
        //        set { SetOperator(value); }
        //      }
        //      public Builder SetOperator(int value) {
        //        PrepareBuilder();
        //        result.hasOperator = true;
        //        result.operator_ = value;
        //        return this;
        //      }
        //      public Builder ClearOperator() {
        //        PrepareBuilder();
        //        result.hasOperator = false;
        //        result.operator_ = 0;
        //        return this;
        //      }

        //      public bool HasAdult {
        //        get { return result.hasAdult; }
        //      }
        //      public bool Adult {
        //        get { return result.Adult; }
        //        set { SetAdult(value); }
        //      }
        //      public Builder SetAdult(bool value) {
        //        PrepareBuilder();
        //        result.hasAdult = true;
        //        result.adult_ = value;
        //        return this;
        //      }
        //      public Builder ClearAdult() {
        //        PrepareBuilder();
        //        result.hasAdult = false;
        //        result.adult_ = false;
        //        return this;
        //      }

        //      public bool HasChannel {
        //        get { return result.hasChannel; }
        //      }
        //      public int Channel {
        //        get { return result.Channel; }
        //        set { SetChannel(value); }
        //      }
        //      public Builder SetChannel(int value) {
        //        PrepareBuilder();
        //        result.hasChannel = true;
        //        result.channel_ = value;
        //        return this;
        //      }
        //      public Builder ClearChannel() {
        //        PrepareBuilder();
        //        result.hasChannel = false;
        //        result.channel_ = 0;
        //        return this;
        //      }
        //    }
        //    static CreateVo() {
        //      object.ReferenceEquals(global::com.tianhe.account.module.account.model.CreateModel.Descriptor, null);
        //    }
        //  }

        //  [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        //  public sealed partial class CreateResult : pb::GeneratedMessage<CreateResult, CreateResult.Builder> {
        //    private CreateResult() { }
        //    private static readonly CreateResult defaultInstance = new CreateResult().MakeReadOnly();
        //    private static readonly string[] _createResultFieldNames = new string[] { "code" };
        //    private static readonly uint[] _createResultFieldTags = new uint[] { 8 };
        //    public static CreateResult DefaultInstance {
        //      get { return defaultInstance; }
        //    }

        //    public override CreateResult DefaultInstanceForType {
        //      get { return DefaultInstance; }
        //    }

        //    protected override CreateResult ThisMessage {
        //      get { return this; }
        //    }

        //    public static pbd::MessageDescriptor Descriptor {
        //      get { return global::com.tianhe.account.module.account.model.CreateModel.internal__static_com_tianhe_account_module_account_model_CreateResult__Descriptor; }
        //    }

        //    protected override pb::FieldAccess.FieldAccessorTable<CreateResult, CreateResult.Builder> InternalFieldAccessors {
        //      get { return global::com.tianhe.account.module.account.model.CreateModel.internal__static_com_tianhe_account_module_account_model_CreateResult__FieldAccessorTable; }
        //    }

        //    public const int CodeFieldNumber = 1;
        //    private bool hasCode;
        //    private int code_;
        //    public bool HasCode {
        //      get { return hasCode; }
        //    }
        //    public int Code {
        //      get { return code_; }
        //    }

        //    public override bool IsInitialized {
        //      get {
        //        if (!hasCode) return false;
        //        return true;
        //      }
        //    }

        //    public override void WriteTo(pb::ICodedOutputStream output) {
        //      int size = SerializedSize;
        //      string[] field_names = _createResultFieldNames;
        //      if (hasCode) {
        //        output.WriteInt32(1, field_names[0], Code);
        //      }
        //      UnknownFields.WriteTo(output);
        //    }

        //    private int memoizedSerializedSize = -1;
        //    public override int SerializedSize {
        //      get {
        //        int size = memoizedSerializedSize;
        //        if (size != -1) return size;

        //        size = 0;
        //        if (hasCode) {
        //          size += pb::CodedOutputStream.ComputeInt32Size(1, Code);
        //        }
        //        size += UnknownFields.SerializedSize;
        //        memoizedSerializedSize = size;
        //        return size;
        //      }
        //    }

        //    public static CreateResult ParseFrom(pb::ByteString data) {
        //      return ((Builder) CreateBuilder().MergeFrom(data)).BuildParsed();
        //    }
        //    public static CreateResult ParseFrom(pb::ByteString data, pb::ExtensionRegistry extensionRegistry) {
        //      return ((Builder) CreateBuilder().MergeFrom(data, extensionRegistry)).BuildParsed();
        //    }
        //    public static CreateResult ParseFrom(byte[] data) {
        //      return ((Builder) CreateBuilder().MergeFrom(data)).BuildParsed();
        //    }
        //    public static CreateResult ParseFrom(byte[] data, pb::ExtensionRegistry extensionRegistry) {
        //      return ((Builder) CreateBuilder().MergeFrom(data, extensionRegistry)).BuildParsed();
        //    }
        //    public static CreateResult ParseFrom(global::System.IO.Stream input) {
        //      return ((Builder) CreateBuilder().MergeFrom(input)).BuildParsed();
        //    }
        //    public static CreateResult ParseFrom(global::System.IO.Stream input, pb::ExtensionRegistry extensionRegistry) {
        //      return ((Builder) CreateBuilder().MergeFrom(input, extensionRegistry)).BuildParsed();
        //    }
        //    public static CreateResult ParseDelimitedFrom(global::System.IO.Stream input) {
        //      return CreateBuilder().MergeDelimitedFrom(input).BuildParsed();
        //    }
        //    public static CreateResult ParseDelimitedFrom(global::System.IO.Stream input, pb::ExtensionRegistry extensionRegistry) {
        //      return CreateBuilder().MergeDelimitedFrom(input, extensionRegistry).BuildParsed();
        //    }
        //    public static CreateResult ParseFrom(pb::ICodedInputStream input) {
        //      return ((Builder) CreateBuilder().MergeFrom(input)).BuildParsed();
        //    }
        //    public static CreateResult ParseFrom(pb::ICodedInputStream input, pb::ExtensionRegistry extensionRegistry) {
        //      return ((Builder) CreateBuilder().MergeFrom(input, extensionRegistry)).BuildParsed();
        //    }
        //    private CreateResult MakeReadOnly() {
        //      return this;
        //    }

        //    public static Builder CreateBuilder() { return new Builder(); }
        //    public override Builder ToBuilder() { return CreateBuilder(this); }
        //    public override Builder CreateBuilderForType() { return new Builder(); }
        //    public static Builder CreateBuilder(CreateResult prototype) {
        //      return new Builder(prototype);
        //    }

        //    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        //    public sealed partial class Builder : pb::GeneratedBuilder<CreateResult, Builder> {
        //      protected override Builder ThisBuilder {
        //        get { return this; }
        //      }
        //      public Builder() {
        //        result = DefaultInstance;
        //        resultIsReadOnly = true;
        //      }
        //      internal Builder(CreateResult cloneFrom) {
        //        result = cloneFrom;
        //        resultIsReadOnly = true;
        //      }

        //      private bool resultIsReadOnly;
        //      private CreateResult result;

        //      private CreateResult PrepareBuilder() {
        //        if (resultIsReadOnly) {
        //          CreateResult original = result;
        //          result = new CreateResult();
        //          resultIsReadOnly = false;
        //          MergeFrom(original);
        //        }
        //        return result;
        //      }

        //      public override bool IsInitialized {
        //        get { return result.IsInitialized; }
        //      }

        //      protected override CreateResult MessageBeingBuilt {
        //        get { return PrepareBuilder(); }
        //      }

        //      public override Builder Clear() {
        //        result = DefaultInstance;
        //        resultIsReadOnly = true;
        //        return this;
        //      }

        //      public override Builder Clone() {
        //        if (resultIsReadOnly) {
        //          return new Builder(result);
        //        } else {
        //          return new Builder().MergeFrom(result);
        //        }
        //      }

        //      public override pbd::MessageDescriptor DescriptorForType {
        //        get { return global::com.tianhe.account.module.account.model.CreateResult.Descriptor; }
        //      }

        //      public override CreateResult DefaultInstanceForType {
        //        get { return global::com.tianhe.account.module.account.model.CreateResult.DefaultInstance; }
        //      }

        //      public override CreateResult BuildPartial() {
        //        if (resultIsReadOnly) {
        //          return result;
        //        }
        //        resultIsReadOnly = true;
        //        return result.MakeReadOnly();
        //      }

        //      public override Builder MergeFrom(pb::IMessage other) {
        //        if (other is CreateResult) {
        //          return MergeFrom((CreateResult) other);
        //        } else {
        //          base.MergeFrom(other);
        //          return this;
        //        }
        //      }

        //      public override Builder MergeFrom(CreateResult other) {
        //        if (other == global::com.tianhe.account.module.account.model.CreateResult.DefaultInstance) return this;
        //        PrepareBuilder();
        //        if (other.HasCode) {
        //          Code = other.Code;
        //        }
        //        this.MergeUnknownFields(other.UnknownFields);
        //        return this;
        //      }

        //      public override Builder MergeFrom(pb::ICodedInputStream input) {
        //        return MergeFrom(input, pb::ExtensionRegistry.Empty);
        //      }

        //      public override Builder MergeFrom(pb::ICodedInputStream input, pb::ExtensionRegistry extensionRegistry) {
        //        PrepareBuilder();
        //        pb::UnknownFieldSet.Builder unknownFields = null;
        //        uint tag;
        //        string field_name;
        //        while (input.ReadTag(out tag, out field_name)) {
        //          if(tag == 0 && field_name != null) {
        //            int field_ordinal = global::System.Array.BinarySearch(_createResultFieldNames, field_name, global::System.StringComparer.Ordinal);
        //            if(field_ordinal >= 0)
        //              tag = _createResultFieldTags[field_ordinal];
        //            else {
        //              if (unknownFields == null) {
        //                unknownFields = pb::UnknownFieldSet.CreateBuilder(this.UnknownFields);
        //              }
        //              ParseUnknownField(input, unknownFields, extensionRegistry, tag, field_name);
        //              continue;
        //            }
        //          }
        //          switch (tag) {
        //            case 0: {
        //              throw pb::InvalidProtocolBufferException.InvalidTag();
        //            }
        //            default: {
        //              if (pb::WireFormat.IsEndGroupTag(tag)) {
        //                if (unknownFields != null) {
        //                  this.UnknownFields = unknownFields.Build();
        //                }
        //                return this;
        //              }
        //              if (unknownFields == null) {
        //                unknownFields = pb::UnknownFieldSet.CreateBuilder(this.UnknownFields);
        //              }
        //              ParseUnknownField(input, unknownFields, extensionRegistry, tag, field_name);
        //              break;
        //            }
        //            case 8: {
        //              result.hasCode = input.ReadInt32(ref result.code_);
        //              break;
        //            }
        //          }
        //        }

        //        if (unknownFields != null) {
        //          this.UnknownFields = unknownFields.Build();
        //        }
        //        return this;
        //      }


        //      public bool HasCode {
        //        get { return result.hasCode; }
        //      }
        //      public int Code {
        //        get { return result.Code; }
        //        set { SetCode(value); }
        //      }
        //      public Builder SetCode(int value) {
        //        PrepareBuilder();
        //        result.hasCode = true;
        //        result.code_ = value;
        //        return this;
        //      }
        //      public Builder ClearCode() {
        //        PrepareBuilder();
        //        result.hasCode = false;
        //        result.code_ = 0;
        //        return this;
        //      }
        //    }
        //    static CreateResult() {
        //      object.ReferenceEquals(global::com.tianhe.account.module.account.model.CreateModel.Descriptor, null);
        //    }
        [ProtoMember(1)]
        public string name;
        [ProtoMember(2)]
        string password;
        [ProtoMember(3)]
        int operator_;
        [ProtoMember(4)]
        bool adult;
        [ProtoMember(5)]
        int channer;
    }

    //  #endregion

}

//#endregion Designer generated code
