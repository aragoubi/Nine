   M S B A M L   `   `   `  ���� B  >NineApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:Nine.Views.ControlsNine.Views.Controls  (    "Nine.Views.Controls.LayerSelection   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     6Controls!clr-namespace:Nine.Views.Controls     c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  .���� $
��Auto=�$��Hidden=�.����   �� Orientation$  
Horizontal=�.+��� ���� *Layers+
 � Tag ��  � FocusVisualStyle Z� ���� .R��� $  
Horizontal=����� .R�� $��4 0 0 0q�.  (System.Windows.Interactivity.Interaction   Triggers /  )System.Windows.Interactivity.EventTrigger     	EventName$ 	MouseDown��.  (System.Windows.Interactivity.TriggerBase   Actions. 6  0System.Windows.Interactivity.InvokeCommandAction     Command �� *Tag.AppBarState.Open+ �� RelativeSource ��  �� AncestorType" ��.+��� -   $	��125��$��Center=�$��Center=�|�. ���� *
Value.Name+	 �� Mode$	 TwoWay=�y��� *Value.IsRenaming+   BooleanToVisibility
 �� 	Converter#
 [  {��� *Value.IsRenaming+�� $	��125��y��� *Value.IsNotRenaming+#
 [  ���� *Key+����  �� Path$ Tag.SelectLayer� �� " ����`�   LayersEqualToStyle `� 	Converter# [  `� ConverterParameter$ LayerSelecter��.1��� $	 Key��� $ Tag.CurrentLayerIndex� �� " ���� $ 
Value.Type�.��� $	��125�� � ColumnDefinitions �� $��18 ��� $��* �.+��� $��0��$��4 0 0 0q�$��11��$��11�����#333y��� *Value.IsDisplayed+#
 [   J� OpacityMask !�  j� Stretch$ Uniform=�  
appbar_eye !� Visual# �  �� $��0��$��4 0 0 0q�$��14��$��14�����#333 J� Opacity$	 0.5Q�y��� *Value.IsDisplayed+#
 [   �� ConverterParameter$ ReverseVisibility�� !� $ Uniform=�  appbar_eye_hide# �  �� $��1��$��Center=�$��0 0 20 0q����� *
Value.Name+�� $��4 0 0 0q�$��50��  ClassicLayerSelecter#��[ ���� *AddNewLayer+.���� ���#222222$��10��$��10�� !� $
 Fill=�  
appbar_add# [ 