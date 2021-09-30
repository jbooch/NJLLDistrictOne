<%@ Page Language="C#" MasterPageFile="~/masterMain.master" AutoEventWireup="true" CodeFile="DistrictStaff.aspx.cs" Inherits="DistrictStaff" Title="District One Staff" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container">
        <div class="row" style="background-color:lavender">
            <div class="col-sm-12 text-sm-center">
                <span style="font-size: 24px; color: #FF0000">New Jersey District 1 Little League
                </span>
                <br />
                <span style="font-size: 18px; color: #000066; font-family: Calibri">District Administrator - Chris Graham
                </span>
                <br />
                <span style="font-size: 18px; color: #000066; font-family: Calibri">NJ District One Staff</span>
                <br />
                <br />
                <a style="font-size: 24px; color: #FF0000" href="Solowey.htm" target="_blank">Tribute to Nick Solowey</a>
                <br />
            </div>
        </div>
    </div>
    <div class="container">
        <br />
        <div class="row">
            <ul class="nav nav-tabs p-2">
                <li class="nav-item ">
                    <asp:Button ID="btnStaff" runat="server" Text="Current Staff" CssClass="btn btn-outline-dark active" OnClick="btnStaff_Click" />
                </li>
                <li class="nav-item">
                    <asp:Button ID="btnPastDA" runat="server" Text ="Past District Admins" CssClass="btn btn-outline-dark" OnClick="btnPastDA_Click" />
                </li>
            </ul>
        </div>
        <br />
        <div class="row">
            <div class="col-sm-12">
                <asp:MultiView ID="MultiView1" runat="server">
                    <asp:View ID="vwStaff" runat="server">
                        <div class="table-responsive">
                            <asp:GridView CssClass="table table-active table-bordered" ID="grdStaff" runat="server" AutoGenerateColumns="False"
                                OnRowEditing="grdStaff_RowEditing">
                                <Columns>
                                    <asp:CommandField ControlStyle-CssClass="btn btn-outline-dark" ButtonType="Button" DeleteText="" InsertText="" NewText="" SelectText=""
                                        ShowEditButton="True" UpdateText="">
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:CommandField>
                                    <asp:TemplateField HeaderText="Name">
                                        <ItemTemplate>
                                            <asp:Label CssClass="text-left" ID="Label1" runat="server" Text='<%# Bind("personURL") %>'></asp:Label>
                                            <asp:Label ID="lblId" runat="server" Text='<% #Bind("id") %>' Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="position" HeaderText="Position"></asp:BoundField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <br />
                    </asp:View>
                    <asp:View ID="vwMaintenance" runat="server">
                        <div class="row border border-dark p-2">
                            <div class="col-sm-12">
                                <div class="form-group row ">
                                    <asp:Label CssClass="col-sm-2 col-form-label-lg" ID="lblPosition" runat="server" Text="Position:"></asp:Label>
                                    <div class="col-sm-6">
                                        <asp:TextBox CssClass="form-control-lg" ID="txtPosition" runat="server" MaxLength="50"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group row">
                                    <asp:Label CssClass="col-sm-2 col-form-label" runat="server" ID="lblName">First Name: </asp:Label>
                                    <div class="col-sm-3">
                                        <asp:TextBox CssClass="form-control" ID="txtFirstName" runat="server"></asp:TextBox>
                                    </div>
                                    <asp:Label CssClass="col-sm-2 col-form-label" runat="server" ID="Label1">Last Name: </asp:Label>
                                    <div class="col-sm-4">
                                        <asp:TextBox CssClass="form-control" ID="txtLastName" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group row">
                                    <asp:Label CssClass="col-sm-2 col-form-label" ID="Label2" runat="server">Home Phone: </asp:Label>
                                    <div class="col-sm-2">
                                        <asp:TextBox CssClass="form-control" placeholder="xxx-xxx-xxxx" ID="txtHomePhone" runat="server"></asp:TextBox>
                                    </div>
                                    <asp:Label CssClass="col-sm-2 col-form-label" ID="Label3" runat="server">Cell Phone: </asp:Label>
                                    <div class="col-sm-2">
                                        <asp:TextBox CssClass="form-control" ID="txtCellPhone" placeholder="xxx-xxx-xxxx" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group row">
                                    <asp:Label CssClass="col-sm-2 col-form-label" ID="Label4" runat="server">Email Address: </asp:Label>
                                    <div class="col-sm-6">
                                        <asp:TextBox CssClass="form-text" ID="txtEmailAddress" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                                <br />
                        <div class="row">
                            <div class="col-sm-12 text-center">
                                <asp:Button CssClass="btn btn-outline-dark" ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
                                <asp:Button CssClass="btn btn-outline-secondary" ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
                            </div>
                        </div>
                    </asp:View>
                    <asp:View ID="vwPAstDa" runat="server">
                        <asp:Repeater ID="rptPastDA" runat="server">
                            <HeaderTemplate>
                                <br />
                                <div class="row">
                                    <div class="offset-4">

                                <h2 class="h2 bg-dark text-white p-1">
                                    <asp:Label ID="Label5" runat="server" Text="Past District Administrators  "></asp:Label>
                                </h2>
                                <ul class="list-group">
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label CssClass="list-inline-item h4 p-2" ID="lblListItem" runat="server" Text='<% #Bind("PastDa") %>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                </ul>
                                    </div>
                                </div>
                            </FooterTemplate>
                        </asp:Repeater>
                    </asp:View>
                </asp:MultiView>
            </div>
        </div>
    </div>
</asp:Content>

