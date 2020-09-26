Public Enum AffinityEnum
    KIND = 2 'Not hostile to other KIND teams
    MEAN = 4 'Hostile to KIND, but not always so other MEAN
    ALOOF = 8 'Alway hostile to other teams
End Enum

Public Class Team
    Public world As World
    Shared last_id As Integer = -1
    Shared available_colors As List(Of Color) = New List(Of Color)

    ' stats
    Public id As Integer
    Public affinity As Integer
    Public color As Color

    ' state
    Public resources As MaterialSet = New MaterialSet()
    Public ship_count_limit As UShort = 8
    Public upgrade_slots_bonus As UShort = 0
    Public ship_count_approximation = 0

    ' IA
    Public bot_team As Boolean = True
    Public upgrade_limit As Integer = 32

    Public Sub New(world As World, affinity As Integer)
        Me.world = world

        'id
        Me.id = last_id + 1
        last_id = Me.id

        'affinity
        If affinity = 0 Then
            If id = 0 Then
                Me.affinity = AffinityEnum.KIND ' player
            ElseIf id = 1 Then
                Me.affinity = AffinityEnum.MEAN ' derelict
            ElseIf id = 2 Then
                Me.affinity = AffinityEnum.ALOOF ' bosses/endgames
            ElseIf id Mod 7 = 0 Then
                Me.affinity = AffinityEnum.KIND
            Else
                Me.affinity = AffinityEnum.MEAN
            End If
        Else
            Me.affinity = affinity
        End If

        'max ships
        If id <> 0 Then
            ship_count_limit = 28
        End If

        'color
        If available_colors.Count = 0 Then
            ' allies colors
            'available_colors.Add(Color.FromArgb(0, 160, 0)) ' dark green (confused with player)
            available_colors.Add(Color.FromArgb(0, 80, 255)) ' deep blue (perfect)
            available_colors.Add(Color.FromArgb(0, 128, 128)) ' dark cyan (perfect)
            available_colors.Add(Color.FromArgb(0, 192, 96)) ' blueish green (good)
            available_colors.Add(Color.FromArgb(128, 128, 255)) ' pale blue (a bit light)
            available_colors.Add(Color.FromArgb(64, 128, 64)) ' desaturated green (looks neutral)
            available_colors.Add(Color.FromArgb(128, 255, 128)) ' pale green (a bit bright)
            available_colors.Add(Color.FromArgb(173, 136, 26)) ' olive (looks yellow)
            available_colors.Add(Color.FromArgb(173, 76, 38)) ' brown (too redish, looks hostile)
            available_colors.Add(Color.FromArgb(128, 0, 255)) ' dark purple (too pinkish)
            'available_colors.Add(Color.FromArgb(128, 128, 128)) ' (confused with neutrals)
            ' enemies colors
            available_colors.Add(Color.FromArgb(173, 34, 69)) ' crismon (pinkish 5th)
            available_colors.Add(Color.FromArgb(255, 128, 255)) ' pink (pinkish 4th)
            available_colors.Add(Color.FromArgb(255, 128, 0)) ' orange (orangish 2nd)
            'available_colors.Add(Color.FromArgb(255, 0, 192)) ' red purple (pinkish 3rd, confusing)
            available_colors.Add(Color.FromArgb(255, 0, 128)) ' red pink (pinkish 2nd)
            available_colors.Add(Color.FromArgb(255, 64, 0)) ' orange-red (orangish 1st)
            available_colors.Add(Color.FromArgb(255, 255, 0)) ' primary yellow
            available_colors.Add(Color.FromArgb(255, 0, 255)) ' primary magenta (pinkish 1st)
            available_colors.Add(Color.FromArgb(255, 48, 48)) ' coral
        End If
        If Me.affinity = AffinityEnum.ALOOF Then
            Me.color = Color.FromArgb(255, 0, 0) ' primary red
        ElseIf Me.id = 0 Then
            Me.color = Color.FromArgb(0, 255, 0) ' primary green
        Else
            Dim i_color As Integer
            If (Me.affinity And AffinityEnum.KIND) <> 0 Then
                i_color = world.Rand.Next(0, available_colors.Count / 4)
            Else
                i_color = world.Rand.Next(available_colors.Count / 4 * 3, available_colors.Count)
            End If
            Me.color = available_colors(i_color)
            available_colors.RemoveAt(i_color)
        End If
    End Sub


    Public Function IsFriendWith(other As Team) As Boolean
        If Me Is other Then
            Return True
        End If
        If other Is Nothing Then
            Return False
        End If
        If (Me.affinity And AffinityEnum.KIND) <> 0 AndAlso (other.affinity And AffinityEnum.KIND) <> 0 Then
            Return True
        End If
        If (Me.affinity And AffinityEnum.MEAN) <> 0 AndAlso (other.affinity And AffinityEnum.MEAN) <> 0 Then
            If (Me.id Mod 6) = (other.id Mod 6) Then
                Return True
            End If
        End If
        Return False
    End Function

End Class
