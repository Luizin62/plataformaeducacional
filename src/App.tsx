import { useState } from 'react';
import { AuthProvider, useAuth } from './contexts/AuthContext';
import { AuthForm } from './components/AuthForm';
import { Layout } from './components/Layout';
import { HomeView } from './views/HomeView';
import { CoursesView } from './views/CoursesView';
import { CourseDetailView } from './views/CourseDetailView';
import { DashboardView } from './views/DashboardView';
import { ProfileView } from './views/ProfileView';

type View = 'home' | 'courses' | 'dashboard' | 'profile' | 'course-detail';

function AppContent() {
  const { user, loading } = useAuth();
  const [currentView, setCurrentView] = useState<View>('home');
  const [selectedCourseId, setSelectedCourseId] = useState<string | null>(null);

  const handleViewCourseDetails = (courseId: string) => {
    setSelectedCourseId(courseId);
    setCurrentView('course-detail');
  };

  const handleBackFromCourseDetail = () => {
    setCurrentView('courses');
    setSelectedCourseId(null);
  };

  if (loading) {
    return (
      <div className="min-h-screen flex items-center justify-center bg-slate-50">
        <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600"></div>
      </div>
    );
  }

  if (!user) {
    return <AuthForm />;
  }

  return (
    <Layout currentView={currentView} onViewChange={setCurrentView}>
      {currentView === 'home' && (
        <HomeView onNavigateToCourses={() => setCurrentView('courses')} />
      )}
      {currentView === 'courses' && (
        <CoursesView onViewDetails={handleViewCourseDetails} />
      )}
      {currentView === 'course-detail' && selectedCourseId && (
        <CourseDetailView
          courseId={selectedCourseId}
          onBack={handleBackFromCourseDetail}
        />
      )}
      {currentView === 'dashboard' && (
        <DashboardView onViewCourseDetails={handleViewCourseDetails} />
      )}
      {currentView === 'profile' && <ProfileView />}
    </Layout>
  );
}

function App() {
  return (
    <AuthProvider>
      <AppContent />
    </AuthProvider>
  );
}

export default App;
