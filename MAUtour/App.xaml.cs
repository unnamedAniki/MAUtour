﻿using MAUtour.Utils.DbConnect;
namespace MAUtour;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
		
		MainPage = new AppShell();
	}
}
