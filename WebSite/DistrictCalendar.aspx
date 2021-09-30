<%@ Page Language="C#" MasterPageFile="~/masterMain.master" AutoEventWireup="true" CodeFile="DistrictCalendar.aspx.cs" Inherits="DistrictCalendar" Title="District Calendar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container">
        <div class="row" style="background-color:lavender">
            <div class="col-sm-12 text-sm-center">
                    <span style="font-size: 24px; color: #FF0000">New Jersey District 1 Little League
                    </span>
                    <br />
                    <span style="font-size: 18px; color: #000066; font-family: Calibri">District Administrator - Chris Graham
                    </span><br />
            <span style="font-size: 18px; color: #000066; font-family: Calibri"> NJ District One Calendar</span><br />
            </div>
        </div>
    </div>
    <div class="container">
        <br />
        <div class="row">
            <div class="col-sm-12">
                <asp:Button CssClass="btn btn-dark" ID="btnAdd" OnClick="btnAdd_Click" runat="server" Text="Add"></asp:Button>
            </div>
        </div>
        <br />
        <asp:UpdatePanel ID="updatePanelOutside" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:MultiView ID="mvCalendar" runat="server">
                    <asp:View ID="vwList" runat="server">
                        <div class="row border border-dark p-2">
                            <div class="col-sm-12">
                                <div class="table-responsive">
                                    <asp:GridView ID="grdCalendar" CssClass="table table-active table-hover table-bordered" runat="server" OnRowDeleting="grdCalendar_RowDeleting" OnRowEditing="grdCalendar_RowEditing" AutoGenerateColumns="False">
                                        <Columns>
                                            <asp:TemplateField ShowHeader="False">
                                                <ItemTemplate>
                                                    <asp:Button CssClass="btn btn-outline-dark" ID="Button1" runat="server" CausesValidation="False" CommandName="Edit"
                                                        Text="Edit" />
                                                    <asp:Button CssClass="btn btn-outline-danger" ID="Button2" runat="server" CausesValidation="False" CommandName="Delete" Text="Delete" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="date" DataFormatString="{0:MMMM dd, yyyy @ hh:mm tt}" HeaderText="When">
                                                <HeaderStyle Font-Bold="True" Font-Underline="True"
                                                    HorizontalAlign="Center" VerticalAlign="Middle" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="subject" HeaderText="What">
                                                <HeaderStyle Font-Bold="True" Font-Underline="True"
                                                    HorizontalAlign="Center" VerticalAlign="Middle" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="place" HeaderText="Where">
                                                <HeaderStyle Font-Bold="True" Font-Underline="True"
                                                    HorizontalAlign="Center" VerticalAlign="Middle" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="description" HeaderText="For">
                                                <HeaderStyle Font-Bold="True" Font-Underline="True"
                                                    HorizontalAlign="Center" VerticalAlign="Middle" />
                                            </asp:BoundField>
                                        </Columns>

                                    </asp:GridView>

                                </div>
                            </div>
                        </div>
                    </asp:View>

                    <asp:View ID="vwMaint" runat="server">
                        <div class="container border-info">
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="form-group row">
                                        <asp:Label ID="Label1" CssClass="col-sm-1 col-form-label" runat="server" Text="When: "></asp:Label>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtDate" CssClass="form-control" runat="server"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtDate" BehaviorID="CustomCalendarExtender" Format="MM/dd/yyyy" PopupPosition="Right"></ajaxToolkit:CalendarExtender>
                                        </div>
                                        <div class="col-sm-3">
                                            <asp:TextBox ID="txtTime" CssClass="form-control" runat="server"></asp:TextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="txtTime" Mask="99:99" MessageValidatorTip="true" MaskType="Time" AcceptAMPM="True"></ajaxToolkit:MaskedEditExtender>
                                            <ajaxToolkit:MaskedEditValidator ID="MaskedEditValidator1" runat="server" ControlExtender="MaskedEditExtender1" ControlToValidate="txtTime" Display="Dynamic" IsValidEmpty="False" EmptyValueMessage="Time is required" InvalidValueMessage="Time is invalid" TooltipMessage="Input a time" EmptyValueBlurredText="*" InvalidValueBlurredMessage="*"></ajaxToolkit:MaskedEditValidator>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="form-group row">
                                        <asp:Label ID="Label2" CssClass="col-sm-1 col-form-label" runat="server" Text="Where:"></asp:Label>
                                        <div class=" col-sm-4">
                                            <asp:TextBox ID="txtPlace" CssClass="form-control" runat="server" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                        <asp:Label ID="Label3" CssClass="col-sm-1 col-form-label" runat="server" Text="What:"></asp:Label>
                                        <div class="col-sm-6">
                                            <asp:TextBox ID="tstSubject" CssClass="form-control" runat="server" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="form-group row">
                                        <asp:Label ID="Label4" CssClass="col-sm-1 col-form-label" runat="server" Text="For:"></asp:Label>
                                        <div class="col-sm-11">
                                            <asp:TextBox ID="txtDescription" CssClass="form-control" runat="server" TextMode="MultiLine" Rows="5"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-8 text-center">
                                    <asp:Button CssClass="btn btn-outline-dark" ID="btnSave" OnClick="btnSave_Click" runat="server" Text="Save"></asp:Button>
                                    <asp:Button CssClass="btn btn-outline-secondary" ID="btnCancel" OnClick="btnCancel_Click" runat="server" Text="Cancel" CausesValidation="False"></asp:Button>
                                </div>
                            </div>
                        </div>
                    </asp:View>
                </asp:MultiView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

