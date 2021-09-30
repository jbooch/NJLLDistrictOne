<%@ Page Language="C#" MasterPageFile="~/masterMain.master" AutoEventWireup="true" CodeFile="MeetingMinutes.aspx.cs" Inherits="MeetingMinutes" Title="Little League District One" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container">
        <div class="row" style="background-color:lavender">
            <div class="col-sm-12 text-sm-center">
                    <span style="font-size: 24px; color: #FF0000">New Jersey District 1 Little League
                    </span>
                    <br />
                    <span style="font-size: 18px; color: #000066; font-family: Calibri">District Administrator - Chris Graham
                    </span><br />
            <span style="font-size: 18px; color: #000066; font-family: Calibri"> NJ District One Meeting Minutes</span><br />
            </div>
        </div>
    </div>
    <div class="container">
        <div class="row">
            <div class="col-sm-12">
                <asp:Repeater ID="rptMinutesYear" runat="server">
                    <HeaderTemplate>
                        <br />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="row">
                            <div class="col-sm-12 text-center">
                                <h4 class="h4">
                                    <asp:Label ID="minuteYear" runat="server" Text='<% #Bind("year") %>'></asp:Label></h4>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-12">
                                <div class="row">
                                    <asp:Repeater ID="rptMinutesMonth" runat="server" DataSource='<% #Bind("months") %>'>
                                        <ItemTemplate>
                                            <div class="col-sm-1">
                                                <asp:Label ID="minuteMonth" runat="server" Text='<%# DataBinder.Eval (Container.DataItem, "month") %>'></asp:Label>
                                            </div>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                    <SeparatorTemplate>
                        <br />
                        <hr />
                        <br />
                    </SeparatorTemplate>
                </asp:Repeater>
            </div>
            <br />
        </div>
        <br />
        <div class="row border border-dark p-2">
            <div class="col-sm-12">
                <asp:Panel ID="minutesAdmin" runat="server">
                    <div class="form-group row">
                        <asp:Label ID="lblyear" runat="server" CssClass="col-sm-1 col-form-label-lg" Text="Year:"></asp:Label>
                        <div class="col-sm-2">
                            <asp:DropDownList ID="dlYear" CssClass="dropdown" runat="server"></asp:DropDownList>
                        </div>
                        <asp:Label ID="lblMonth" runat="server" CssClass="col-sm-1 col-form-label" Text="Month:"></asp:Label>
                        <div class="col-sm-2">
                            <asp:DropDownList ID="dlMonth" CssClass="dropdown" runat="server"></asp:DropDownList>
                        </div>
                        <asp:Label ID="lblFile" runat="server" CssClass="col-sm-1 col-form-label" Text="File:"></asp:Label>
                        <div class="col-sm-4">
                            <asp:UpdatePanel ID="updFileLoad" runat="server" UpdateMode="Always">
                                <ContentTemplate>
                                    <ajaxToolkit:AsyncFileUpload ID="fleMinutes" runat="server" OnUploadedFileError="fleMinutes_UploadedFileError" OnUploadedComplete="fleMinutes_UploadedComplete" UploaderStyle="Modern" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <asp:Button ID="btnSave" CssClass="btn btn-outline-dark" runat="server" Text="Save" OnClick="btnSave_Click" />
                    </div>
                </asp:Panel>
            </div>
        </div>
        <br />
    </div>
</asp:Content>

