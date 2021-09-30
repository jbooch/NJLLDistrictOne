<%@ Page Language="C#" MasterPageFile="~/masterMain.master" AutoEventWireup="true" CodeFile="Umpires.aspx.cs" Inherits="Umpires" Title="District One Umpires" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container">
        <div class="row" style="background-color:lavender">
            <div class="col-sm-12 text-sm-center">
                    <span style="font-size: 24px; color: #FF0000">New Jersey District 1 Little League
                    </span>
                    <br />
                    <span style="font-size: 18px; color: #000066; font-family: Calibri">District Administrator - Chris Graham
                    </span><br />
            <span style="font-size: 18px; color: #000066; font-family: Calibri"> NJ District One Umpires</span><br />
            </div>
        </div>
    </div>
        <asp:MultiView ID="mvUmps" runat="server">
            <asp:View ID="View1" runat="server">
                <div class="container">
                    <div class="row border border-dark p-2">
                        <div class="col-sm-12">
                            <div class="row">
                                <div class="col-sm-12 text-center">
                                    <asp:Button CssClass="btn btn-outline-primary" ID="btnAdd" runat="server" Text="Add" OnClick="btnAdd_Click" />
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="table-responsive">
                                        <asp:GridView CssClass="table table-bordered table-active" ID="grdUmps" runat="server" AutoGenerateColumns="False" OnRowEditing="grdUmps_RowEditing">
                                            <Columns>
                                                <asp:CommandField ControlStyle-CssClass="btn btn-outline-dark" ButtonType="Button" CancelText="" CausesValidation="False" DeleteText="Remove"
                                                    InsertText="" InsertVisible="False" NewText="" SelectText="" ShowCancelButton="False"
                                                    ShowDeleteButton="True" ShowEditButton="True" />
                                                <asp:TemplateField HeaderText="Name">
                                                    <ItemTemplate>
                                                        <asp:HyperLink CssClass="btn btn-outline-info" ID="lblName" runat="server" NavigateUrl='<%#Bind("umpireDetailLink") %>' Text='<%# Bind("fullName") %>' Target="_blank"></asp:HyperLink>
                                                    </ItemTemplate>
                                                    <HeaderStyle Font-Bold="True" Font-Underline="True"
                                                        HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    <ItemStyle  HorizontalAlign="Left" VerticalAlign="Middle" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="umpireSince" HeaderText="Umpire Since">
                                                    <HeaderStyle Font-Bold="True" Font-Underline="True"
                                                        HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="homeLeague" HeaderText="Home League">
                                                    <HeaderStyle Font-Bold="True" Font-Underline="True"
                                                        HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="credits" HeaderText="Credits">
                                                    <HeaderStyle Font-Bold="True" Font-Underline="True"
                                                        HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                </asp:BoundField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:View>
            <asp:View ID="View2" runat="server">
                <div class="container">
                    <br />
                    <div class="row border border-dark p-2">
                        <div class="col-sm-12">
                            <div class="row p-2">
                                <div class=" col-sm-5">
                                    <div class="input-group">
                                        <span class="input-group-text">First Name: </span>
                                        <asp:TextBox CssClass="form-control" ID="txtFirstName" placeholder="First" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-sm-7 input-group">
                                    <span class="input-group-text">Last Name:</span>
                                    <asp:TextBox CssClass="form-control" ID="txtLastName" placeholder="Last" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row p-2">
                                <div class=" col-sm-7">
                                    <div class="input-group">
                                        <span class="input-group-text">HomeLeague:</span>
                                        <asp:TextBox CssClass="form-control" ID="txtHomeLeague" placeholder="League Name" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-sm-5">
                                    <div class="input-group">
                                        <span class="input-group-text">Umpire Since:</span>
                                        <asp:TextBox CssClass="form-control" ID="txtUmpireSince" placeholder="YYYY" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="row p-2">
                                <div class="col-sm-8">
                                    <div class="input-group">
                                        <span class="input-group-text">Credits:</span>
                                        <asp:TextBox CssClass="form-control" ID="txtCredits" runat="server" Rows="3" TextMode="MultiLine"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="row p-2">
                                <div class="col-sm-6">
                                    <asp:Image CssClass="img-thumbnail" ID="imgUmpire" runat="server" />
                                    <asp:CheckBox ID="ckRemove" runat="server" Text="Remove Image" />
                                </div>
                                <div class="col-sm-6">
                                    <div class="input-group">
                                        <span class="input-group-text">Profile Image</span>
                                        <input class="form-control" id="flDefaultImage" runat="server" type="file" />
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-6 text-center">
                                    <asp:Button CssClass="btn btn-outline-dark" ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
                                </div>
                                <div class="col-sm-6 text-center">
                                    <asp:Button CssClass="btn btn-outline-secondary" ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>            
            </asp:View>
        </asp:MultiView>
</asp:Content>

