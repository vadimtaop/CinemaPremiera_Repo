   M S B A M L   `   `   `  ����I  ECinemaPremiera, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:CinemaPremieraCinemaPremiera  5   	       CinemaPremiera.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   .localclr-namespace:CinemaPremiera     5   	   $	��650��5   	   $
��1200��6     � Icon$  /images/logo.png��6)    � Title$+ %Кинотеатр "Премьера"��5	   	   $	��750��6&   $
��1300��63    � WindowStartupLocation$ CenterScreen=�6@   ��5
      �� 5   
    �� EndPoint$ 0.5,1&�6    �� 
StartPoint$	 0,0&�6/   .��5      �  � Color$ 	#FF23537D��6    � Offset$ 0.006Q�6-   � 5      $ 	#FF092B46��6   $ 1Q�6-   � 5      $ 	#FF275277��6   $	 0.5Q�6-   5      .��5      �  � RowDefinitions 5   
   �� 5      $��75 �6   �� 5      $��* �6   .+�5   
   � $��0��6   ���#9ebed9$��Stretch=�5      $��Stretch=�6+    � ColumnDefinitions 5      �� 5      $	��100 �6#   �� 5      $��* �6#   �� 5      $	��100 �6#   .+�5      �� $��images/logo.png��6   $��55��6-   $��5q�69   $��Center=�5       $��Center=�61   �� 5!      $+��%Кинотеатр "Премьера"��6   
�������$��34(�5"      $��DemiBold"�6&   $��1��6<   $��Center=�5#      $��Center=�65   	 J� Effect	 5$      3  -System.Windows.Media.Effects.DropShadowEffect  5%      
   
BlurRadius$
 0Q�6'      ShadowDepth$ 2Q�66   5'      �� 5(      -   5        $��Btn_Exit��5,      $��50��5(      $��50��6!   $��0q�5)      
������ $��2��5*      $��10q�6%   $y�	Collapsed=�61   $��Center=�5+      $��Center=�62   .��5-      �� $��images/Door_Exit.png��6   5.      5/       � 51   
     � Source$ pages/PincodePage.xaml?�6     � NavigationUIVisibility$ Hidden=�60   $��1��52      53      54      