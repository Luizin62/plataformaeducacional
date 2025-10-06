import { createClient } from '@supabase/supabase-js';

const supabaseUrl = import.meta.env.VITE_SUPABASE_URL;
const supabaseAnonKey = import.meta.env.VITE_SUPABASE_ANON_KEY;

export const supabase = createClient(supabaseUrl, supabaseAnonKey);

export type Profile = {
  id: string;
  full_name: string;
  avatar_url: string | null;
  role: 'student' | 'instructor' | 'admin';
  created_at: string;
  updated_at: string;
};

export type Course = {
  id: string;
  title: string;
  description: string;
  instructor_id: string;
  thumbnail_url: string | null;
  level: 'beginner' | 'intermediate' | 'advanced';
  category: string;
  published: boolean;
  created_at: string;
  updated_at: string;
};

export type Lesson = {
  id: string;
  course_id: string;
  title: string;
  content: string;
  video_url: string | null;
  order_index: number;
  duration_minutes: number | null;
  created_at: string;
};

export type Enrollment = {
  id: string;
  student_id: string;
  course_id: string;
  enrolled_at: string;
  completed: boolean;
  progress: number;
};

export type LessonProgress = {
  id: string;
  enrollment_id: string;
  lesson_id: string;
  completed: boolean;
  completed_at: string | null;
};
