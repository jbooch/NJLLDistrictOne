<%@ Page Title="New Jersey District One" Language="C#" MasterPageFile="~/masterMain.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="CardMaintenance.aspx.cs" Inherits="CardMaintenance"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container">
        <div class="row" style="background-color: lavender">
            <div class="col-sm-12 text-sm-center">
                <span style="font-size: 24px; color: #FF0000">New Jersey District 1 Little League
                </span>
                <br />
                <span style="font-size: 18px; color: #000066; font-family: Calibri">District Administrator - Chris Graham
                </span>
                <br />
                <span style="font-size: 18px; color: #000066; font-family: Calibri">NJ District One Card Maintenance</span><br />
            </div>
        </div>
    </div>
    <div class="container">
<!--        <asp:UpdatePanel ID="updatePanelOutside" runat="server" UpdateMode="Conditional">
           <ContentTemplate> -->
                <br />
                <div class="row border border-dark p-2">
                    <div class="col-sm-12">
                        <div class="row">
                            <div class="col-sm-12 text-center">
                                <asp:ListBox ID="lstCards" runat="server" CssClass="list-group-horizontal-sm" AutoPostBack="True" OnTextChanged="lstCards_TextChanged"></asp:ListBox>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-sm-6">
                                <div class="input-group">
                                    <asp:Label CssClass="input-group-text" ID="lblTitle" runat="server">Card Title: </asp:Label>
                                    <asp:TextBox CssClass="form-control" Placeholder="Card Title" ID="txtTitle" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator CssClass="input-group-text" ID="RequiredFieldValidator2" runat="server" ErrorMessage="Card Title is Required" ControlToValidate="txtTitle" Text="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <div class="input-group">
                                    <asp:Label CssClass="input-group-text" ID="lblCardNumber" runat="server">Card Number: </asp:Label>
                                    <asp:TextBox CssClass="form-control" placeholder="XX" ID="txtCardNumber" runat="server"></asp:TextBox>
                                    <asp:RangeValidator CssClass="input-group-text" ID="RangeValidator1" runat="server" ControlToValidate="txtCardNumber" ErrorMessage="Invalid Number" SetFocusOnError="True" Type="Integer" Text="*" MaximumValue="99" MinimumValue="1" Display="Dynamic"></asp:RangeValidator>
                                    <asp:RequiredFieldValidator CssClass="input-group-text" ID="RequiredFieldValidator1" runat="server" ErrorMessage="Card Number is Required" ControlToValidate="txtCardNumber" Text="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <div class="input-group">
                                     <asp:Label CssClass="input-group-text" ID="lblPriority" runat="server" Text="Priority: "></asp:Label>
                                    <asp:RadioButtonList CssClass="form-control" ID="rbPriority" runat="server">
                                        <asp:ListItem class="text-white bg-info p-1" Value="Info">Informational</asp:ListItem>
                                        <asp:ListItem class="text-white bg-primary p-1">High</asp:ListItem>
                                        <asp:ListItem class="text-white bg-danger p-1">Hot</asp:ListItem>
                                        <asp:ListItem class="text-white bg-success p-1">Success</asp:ListItem>
                                        <asp:ListItem class="text-white bg-warning p-1">Warning</asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-sm-6">
                                <div class="input-group">
                                    <asp:Label CssClass="input-group-text" ID="lblBody" runat="server">Card Text</asp:Label>
                                    <asp:TextBox CssClass="form-control" ID="txtBody" runat="server" TextMode="MultiLine"></asp:TextBox>
                                </div>
                                <div class="col-sm=3">
                                    <div class="input-group">
                                        <asp:Label CssClass="input-group-text" ID="lblImg" runat="server">Image Upload</asp:Label>
                                        <asp:UpdatePanel ID="updateFileUpload" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <input class="form-control" id="flImg" runat="server" type="file"/>
                                            </ContentTemplate>
                                        </asp:UpdatePanel> 
                                        <asp:Image CssClass="img-fluid" ID="cdImg" runat="server" AlternateText="Card Image" Width="250px" />
                                        <asp:CheckBox ID="ckRemoveImage" runat="server" Text="Remove Image" TextAlign="Right" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="offset-2 col-sm=10">
                                <asp:ValidationSummary CssClass="text-danger" ID="ValidationSummary1" runat="server" />
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-sm-12 text-center">
                                <asp:Button ID="btnSave" runat="server" CssClass="btn btn-outline-dark" OnClick="btnSave_Click" />
                                <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btn btn-outline-danger" Enabled="false" OnClick="btnDelete_Click" CausesValidation="False" />
                            </div>
                        </div>
                    </div>
                </div>
<!--            </ContentTemplate>
        </asp:UpdatePanel>  -->
    </div>
</asp:Content>
