   M S B A M L   `   `   `  ����A  =VST111, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:VST111.ViewsVST111.Views  5   	       VST111.Views.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ,localclr-namespace:VST111.Views     5   	     � Title$  
MainWindow��5   	   $	��450��6   $	��800��6)   .��5	      ]� .+�5      ]� 
����+��$��50��6&   $	��Top=�62   .+�5      �� $��Top Bar��6   
���   �$��14(�61   $
��Bold"�6?   $��	10,15,0,0q�6Q   �� 5      -   5        $��	SearchBox��5      $	��200��6   $��25��6*   $��
300,12,0,0q�66   $	��Top=�6J   $
��Left=�6b   $��	Search...��6}   
�������6�   5      ]� 5      
����ح�$	��200��6%   $
��Left=�61   .+�5      �� .+�5      �� $
��Logo��6   $��14(�6   $
��Bold"�6*   $��	10,10,0,0q�6<   �� 5      $��Sidebar��6   $��10(�6   $��	10,50,0,0q�6-   �� 5      -   5        $��
Function 1��5      $��
10,100,0,0q�6"   $	��150��66   6B   �� 5      $��
Function 2��6   $��
10,140,0,0q�6"   $	��150��66   d� 5      -   5         d�Name$ ToggleAdditionalFunctions��5      $��	Show More��66   $��
10,220,0,0q�6J   $	��150��6^   �� 5      -   5         ��Name$ AdditionalFunctionsPanel��5      $y�	Collapsed=�63   .+�5      �� $��
Function 4��6   $��	10,10,0,0q�6#   $	��150��66   �� 5      $��
Function 5��6   $��	10,10,0,0q�6#   $	��150��66   �� 5      $��
Function 6��6   $��	10,10,0,0q�6#   $	��150��66   5       5!      5"      � 5%      
�������.+�5&      �� $��Main Content Area��6   $��12(�6(   $��
10,10,0,10q�66   &   System.Windows.Controls.DataGrid  5'      -   5          Name$ DataGridTable��5'      $��10,40,10,10q�6%      AutoGenerateColumns . $y�	Collapsed=�6U   5)      5*      5+      